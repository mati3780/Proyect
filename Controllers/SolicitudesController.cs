using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Catpcha;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.RuleFilter;
using PROYECT.WebAPI.Extensions.Validation;
using PROYECT.Helpers.Filters;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Solicitudes")]
    public class SolicitudesController : ReportController
    {
        #region Constructor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorio<Solicitud> _solicitudRepositorio;
        private readonly IRepositorio<TipoEstadoTramite> _tipoRepositorio;
        private readonly IRepositorio<SolicitudRectificacion> _solicitudRectificacion;
        private readonly ITipoEstadoTramiteRepository _tipoEstadoTramiteRepositorio;
        private readonly ISubTipoEstadoTramiteJurisdiccionRepositorio _subTipoEstadoTramiteJurisdiccionRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IRepositorio<Servicio> _servicioRepositorio;
        private readonly ITasaNacionalRepositorio _tasaNacionalRepositorio;
        private readonly IRepositorio<Feriado> _feriadoRepositorio;
        private readonly IValidator<SolicitudDto> _validator;

        public SolicitudesController(IUnitOfWork unitOfWork, IRepositorio<Solicitud> solicitudRepositorio, IRepositorio<SolicitudRectificacion> solicitudRectificacion,
                                  ITipoEstadoTramiteRepository tipoEstadoTramiteRepositorio, ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio,
                                  IRepositorio<Jurisdiccion> jurisdiccionRepositorio, IValidator<SolicitudDto> validator, ITasaNacionalRepositorio tasaNacionalRepositorio,
                                  IRepositorio<Servicio> servicioRepositorio, IRepositorio<Feriado> feriadoRepositorio, IRepositorio<TipoEstadoTramite> tipoRepositorio)
        {
            _unitOfWork = unitOfWork;
            _solicitudRepositorio = solicitudRepositorio;
            _solicitudRectificacion = solicitudRectificacion;
            _validator = validator;
            _tasaNacionalRepositorio = tasaNacionalRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _feriadoRepositorio = feriadoRepositorio;
            _tipoRepositorio = tipoRepositorio;
            _subTipoEstadoTramiteJurisdiccionRepositorio = subTipoEstadoTramiteJurisdiccionRepositorio;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _tipoEstadoTramiteRepositorio = tipoEstadoTramiteRepositorio;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Route("Search")]
        public IHttpActionResult Search(SolicitudBusquedaDto criterio)
        {
            if (User.IsAdmin())
                return NotFound();

            var jurisdiccion = _jurisdiccionRepositorio.Get(User.GetJurisdiccionId());
            var query = _solicitudRepositorio.GetAll();

            if (jurisdiccion.IsBloqueadoRequiriente())
                return NotFound();

            query = query.Filter(criterio);

            var page = query.OrderBy(x => x.Fecha).ToPage(criterio.Start, criterio.Length, r => r.MapResumen());

            return Ok(page);
        }

        [Route("{id:long}")]
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            var result = _solicitudRepositorio.Get(id);

            if (!User.IsAdmin() && (result == null || !(result.JurisdiccionRequirenteId == User.GetJurisdiccionId() || result.Tramites.Any(t => t.JurisdiccionId == User.GetJurisdiccionId()))))
                return NotFound();

            var dto = result.Map();
            dto.FechaValidez = GetFechaValidez(result).ToShort();

            return Ok(dto);
        }

        [Route("{id:long}/Rectificatorias")]
        public IHttpActionResult GetRectificatorias(Int64 id)
        {
            var results = _solicitudRectificacion.GetAll().Where(x => x.SolicitudId == id).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }

        [Route("Report/{id:long}")]
        [HttpGet]
        public async Task<IHttpActionResult> Report(long id)
        {
            var solicitud = _solicitudRepositorio.Get(id);
            if (solicitud == null)
                return NotFound();

            return await GetReportToDownload(solicitud, false, GetFechaValidez(solicitud));
        }

        [Route("ReportData/{id:long}")]
        [RequireGuidAuthentication]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult ReportData(int id)
        {
            var result = _solicitudRepositorio.Get(id);

            if (result == null)
                return NotFound();

            var dto = result.Map();
            dto.FechaValidez = GetFechaValidez(result).ToShort();

            return Ok(dto);
        }

        [Route("")]
        [HttpPost]
        [CaptchaValidator]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody] SolicitudDto model)
        {
            var servicio = _servicioRepositorio.Get(model.ServicioId);
            if (servicio.Inmueble)
                model.Entidad.Persona = null;
            else
                model.Entidad.Inmueble = null;

            var solicitud = model.Map(null);
            solicitud.DatosAdicionales = model.Observacion.TrimSafe();

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            #region Set Default

            solicitud.AsignarNumero();
            solicitud.TasaNacionalId = _tasaNacionalRepositorio.GetVigenteId();
            var estadoId = _tipoEstadoTramiteRepositorio.GetIdPorIdetificador(TipoEstadoSolicitudIdentificador.PresentacionPendiente);
            var subEstadoId = _subTipoEstadoTramiteJurisdiccionRepositorio.GetIdPorIdetificador(SubTipoEstadoTramiteIdentificador.Pendiente);
            solicitud.AddEstado(estadoId, subEstadoId);

            #endregion

            Save(solicitud);

            //Usado para la generación del reporte
            solicitud.JurisdiccionRequirente = _jurisdiccionRepositorio.Get(solicitud.JurisdiccionRequirenteId);

            return await GetReportToDownload(solicitud, true, GetFechaValidez(solicitud));
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]SolicitudDto model)
        {
            var solicitud = _solicitudRepositorio.Get(id);

            if (solicitud == null || solicitud.IsFinalizada())
                return NotFound();

            model.Id = id;

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            if (model.IsRectificacion())
            {
                var solicitudRectificada = solicitud.MapRectificacion(model);
                _solicitudRectificacion.Add(solicitudRectificada);
            }

            var estadoId = _tipoEstadoTramiteRepositorio.GetIdPorIdetificador(TipoEstadoSolicitudIdentificador.AprobadoRegistrado);
            var subEstadoId = _subTipoEstadoTramiteJurisdiccionRepositorio.GetIdPorIdetificador(SubTipoEstadoTramiteIdentificador.EnTramite);
            solicitud.AddEstado(estadoId, subEstadoId, model.ParseObservaciones());

            model.Map(solicitud);

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("Consulta")]
        [HttpPost]
        [CaptchaValidator]
        [AllowAnonymous]
        public IHttpActionResult Consulta([FromBody] ConsultaDto model)
        {
            var solicitud = _solicitudRepositorio.GetAll().SingleOrDefault(x => x.Numero.Equals(model.Numero));

            if (solicitud == null)
                return NotFound();

            var feriadosNacionales = _feriadoRepositorio.GetAll().Where(x => x.Jurisdiccion == null).ToList();

            return Ok(solicitud.MapConsulta(feriadosNacionales));
        }

        #endregion

        #region Private Methods

        private void Save(Solicitud solicitud)
        {
            var retries = 3;
            while (true)
            {
                try
                {
                    _solicitudRepositorio.Add(solicitud);
                    _unitOfWork.SaveChanges();

                    break;
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException == null || !ex.InnerException.InnerException.Message.Contains("IX_Tramite_UniqueNumero"))
                        throw;

                    if (--retries == 0)
                        throw;

                    solicitud.AsignarNumero();
                }
            }
        }

        private DateTime GetFechaValidez(Solicitud solicitud)
        {
            var feriadosNacionales = _feriadoRepositorio.GetAll().Where(x => x.Jurisdiccion == null).ToList();
            return FeriadoHelper.GetFechaValidez(solicitud, feriadosNacionales);
        }

        #endregion
    }
}
