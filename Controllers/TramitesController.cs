using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Email;
using PROYECT.Helpers.Pdf;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Enums;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.Common;
using PROYECT.WebAPI.Extensions.RuleFilter;
using PROYECT.WebAPI.Extensions.Validation;
using PROYECT.WebAPI.Handler;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Tramites")]
    public class TramitesController : ReportController
    {
        #region Constructor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorio<Tramite> _tramiteRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IRepositorio<Feriado> _feriadoRepositorio;
        private readonly ISubTipoEstadoTramiteJurisdiccionRepositorio _subTipoEstadoTramiteJurisdiccionRepositorio;
        private readonly IRepositorio<SolicitudRectificacion> _solicitudRectificacionRepositorio;
        private readonly IRepositorio<Documento> _documentoRepositorio;
        private readonly IValidator<TramiteEditDto> _validator;
        private readonly IList<ITramiteActionHandler> _actionHandlers;

        public TramitesController(IUnitOfWork unitOfWork, IRepositorio<Tramite> tramiteRepositorio,
                                                IRepositorio<Feriado> feriadoRepositorio, ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio,
                                                IRepositorio<Jurisdiccion> jurisdiccionRepositorio, IRepositorio<SolicitudRectificacion> solicitudRectificacionRepositorio,
                                                IRepositorio<Documento> documentoRepositorio, IValidator<TramiteEditDto> validator, IList<ITramiteActionHandler> actionHandlers)
        {
            _unitOfWork = unitOfWork;
            _tramiteRepositorio = tramiteRepositorio;
            _feriadoRepositorio = feriadoRepositorio;
            _subTipoEstadoTramiteJurisdiccionRepositorio = subTipoEstadoTramiteJurisdiccionRepositorio;
            _solicitudRectificacionRepositorio = solicitudRectificacionRepositorio;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _validator = validator;
            _actionHandlers = actionHandlers;
            _documentoRepositorio = documentoRepositorio;
        }

        #endregion

        [Route("Search/{tipo}")]
        [HttpPost]
        public IHttpActionResult Search(TramiteBusquedaDto criterio, ServicioGrupo tipo)
        {
            if (User.IsAdmin())
                return NotFound();

            var jurisdiccionId = User.GetJurisdiccionId();
            var jurisdiccion = _jurisdiccionRepositorio.Get(jurisdiccionId);
            var feriadosNacionales = _feriadoRepositorio.GetAll().Where(x => x.Jurisdiccion == null).ToList();

            var query = _tramiteRepositorio.GetAll();

            switch (tipo)
            {
                case ServicioGrupo.Requiriente:
                    {
                        if (jurisdiccion.IsBloqueadoRequiriente())
                            return NotFound();

                        return Ok(query.GetRequiriente(criterio, feriadosNacionales));
                    }
                case ServicioGrupo.Informante:
                    {
                        if (jurisdiccion.IsBloqueadoInformante())
                            return NotFound();

                        var informante = query.GetInformante(criterio, feriadosNacionales);
                        return Ok(informante);
                    }
            }

            return NotFound();
        }

        [Route("SearchAdminEnte")]
        [HttpPost]
        public IHttpActionResult SearchAdminEnte(TramiteBusquedaAdminEnteDto criterio)
        {
            //si no es admin solo puede buscar dentro de sus jurisdicciones
            if (!User.IsAdmin() && !User.IsEnte())
                criterio.Jurisdiccion = User.GetJurisdiccionId();

            var query = _tramiteRepositorio.GetAll();
            query = query.FilterAdminEnte(criterio);

            //los busco en el controller porque en el dto no puedo injectar
            var feriadosNacionales = _feriadoRepositorio.GetAll().Where(x => x.Jurisdiccion == null).ToList();

            var page = query.OrderBy(x => x.Solicitud.Fecha).ToPage(criterio.Start, criterio.Length, r => r.MapResumenAdmin(feriadosNacionales));

            return Ok(page);
        }

        [Route("{id:long}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            #region
            //var result = _tramiteRepositorio.Get(id);

            //if (result == null)
            //    return NotFound();

            //var feriadosNacionales = _feriadoRepositorio.GetAll().Where(x => x.Jurisdiccion == null).ToList();

            //return Ok(result.MapResumen(feriadosNacionales));
            #endregion

            var result = _tramiteRepositorio.Get(id);
            if (!User.IsAdmin() && (result == null || !(result.Jurisdiccion.Id == User.GetJurisdiccionId() || result.Solicitud.JurisdiccionRequirente.Id == User.GetJurisdiccionId())))
                return NotFound();

            var item = result.Map();

            return Ok(item);
        }

        [Route("{id:long}/Informe")]
        [HttpGet]
        public IHttpActionResult Informe(int id)
        {
            var result = _tramiteRepositorio.Get(id);

            if (!User.IsAdmin() && (result == null || !(result.Jurisdiccion.Id == User.GetJurisdiccionId() || result.Solicitud.JurisdiccionRequirente.Id == User.GetJurisdiccionId())))
                return NotFound();

            var informe = _documentoRepositorio.GetAll().SingleOrDefault(x => x.TramiteId == id);

            if (informe == null)
                return NotFound();

            return new ResponseMessageResult(informe.Archivo.ToPdfResponseMessage("Informe"));
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]TramiteEditDto model)
        {
            var tramite = _tramiteRepositorio.Get(id);

            if (tramite == null || tramite.IsFinalizado())
                return NotFound();

            model.Id = id;

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            if (model.IsRectificacion())
            {
                var solicitudRectificada = tramite.Solicitud.MapRectificacion(model);
                solicitudRectificada.TramiteId = tramite.Id;
                _solicitudRectificacionRepositorio.Add(solicitudRectificada);
            }

            var subEstado = _subTipoEstadoTramiteJurisdiccionRepositorio.GetPorIdetificador(SubTipoEstadoTramiteIdentificador.Rectificado);
            tramite.AddEstadoTramite(new TramiteEstado { SubTipo = subEstado, Observacion = model.ParseObservaciones() });

            model.Map(tramite);

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}/Accion/{subtipo}")]
        [HttpPut]
        public IHttpActionResult Accion(Int64 id, SubTipoEstadoTramiteOption subtipo, [FromBody]TramiteActionDto model)
        {
            if (model == null)
                model = new TramiteActionDto();

            model.TramiteId = id;

            var tramite = _tramiteRepositorio.Get(model.TramiteId);

            if (tramite == null)
                return NotFound();

            var handler = (TramiteHandlerBase)_actionHandlers.Single(x => x.GetType() == subtipo.GetTramiteHandler());

            if (handler.Valid(model, ModelState))
                BadRequest(ModelState);

            handler.Set(tramite, model, new byte[0]);

            _unitOfWork.SaveChanges();

            handler.SendEmail(tramite);

            return Ok();
        }

        [Route("{id:long}/Informar")]
        [HttpPut]
        public IHttpActionResult Informar(Int64 id)
        {
            byte[] httpPostedFile = null;

            if (HttpContext.Current.Request.Files["file"] != null)
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                httpPostedFile = HttpContext.Current.Request.Files["file"].ToBytes();
                if (PdfHelper.IsSigned(httpPostedFile))
                    if (!PdfHelper.HasValidSignature(httpPostedFile))
                    {
                        ModelState.AddModelError(String.Empty, "La firma digital del PDF no es válida o se ha vencido.");
                        return BadRequest(ModelState);
                    }
            }

            var model = new TramiteActionDto { TramiteId = id };

            var tramite = _tramiteRepositorio.Get(model.TramiteId);

            if (tramite == null)
                return NotFound();

            var handler = (TramiteHandlerBase)_actionHandlers.Single(x => x.GetType() == SubTipoEstadoTramiteOption.SeRegistroPdf.GetTramiteHandler());

            if (handler.Valid(model, ModelState))
                BadRequest(ModelState);

            handler.Set(tramite, model, httpPostedFile);

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("Report/{id:long}")]
        [HttpGet]
        public async Task<IHttpActionResult> Report(long id)
        {
            var tramite = _tramiteRepositorio.Get(id);
            if (tramite == null)
                return NotFound();

            return await GetReportToDownload(tramite.Solicitud, false);
        }
    }
}