using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Filters;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.Common;
using PROYECT.WebAPI.Extensions.RuleFilter;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Liquidaciones")]
    public class LiquidacionesController : ReportController
    {
        #region Constructor

        private readonly IRepositorio<Liquidacion> _liquidacionRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LiquidacionDto> _validator;
        private readonly IValidator<CompensacionDto> _compensacionValidator;
        private readonly IRepositorio<Compensacion> _compensacionRepositorio;

        public LiquidacionesController(IUnitOfWork unitOfWork, IRepositorio<Liquidacion> liquidacionRepositorio, IRepositorio<Jurisdiccion> jurisdiccionRepositorio, IValidator<LiquidacionDto> validator, IValidator<CompensacionDto> compensacionValidator, IRepositorio<Compensacion> compensacionRepositorio)
        {
            _unitOfWork = unitOfWork;
            _liquidacionRepositorio = liquidacionRepositorio;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _validator = validator;
            _compensacionValidator = compensacionValidator;
            _compensacionRepositorio = compensacionRepositorio;
        }

        #endregion

        [HttpPost]
        [Route("SearchLiquidaciones")]
        public IHttpActionResult SearchLiquidaciones(SearchLiquidacionesDTO criterio)
        {
            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();

            if (User.IsAdmin() || User.IsEnte())
            {
                if (!criterio.JurisdiccionId.HasValue)
                {
                    if (User.IsAdmin())
                    {
                        //Si la jurisdiccion es Proyect tiene que ver y comportarse como si fuese el Ente. ELR (comportamiento extraido de la logica del viejo sinarepi) 
                        criterio.JurisdiccionId = _jurisdiccionRepositorio.GetAll().Single(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).Id;
                    }
                    else
                    {
                        criterio.JurisdiccionId = User.GetJurisdiccionId();
                    }
                }
                else if (criterio.JurisdiccionId.Value == _jurisdiccionRepositorio.GetAll().Single(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).Id)
                {
                    criterio.EsEnteOrigenDestino = true;
                }
            }
            else
            {
                criterio.JurisdiccionId = User.GetJurisdiccionId();
            }


            return SearchLiquidacionesBase(criterio, jurisdiccionesEnte);
        }

        [HttpPost]
        [Route("SearchLiquidacionesPendientes")]
        public IHttpActionResult SearchLiquidacionesPendientes(SearchLiquidacionesDTO criterio)
        {
            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var query = _liquidacionRepositorio.GetAll().Where(x => (x.OrigenId == criterio.JurisdiccionId || x.DestinoId == criterio.JurisdiccionId) && x.Compensacion == null).ToList();
            return Ok(query.Select(x => x.MapListItem(User.GetJurisdiccionId(), jurisdiccionesEnte)));
        }

        private IHttpActionResult SearchLiquidacionesBase(SearchLiquidacionesDTO criterio, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            IQueryable<Liquidacion> query;

            if (criterio.EsEnteOrigenDestino)
            {
                query = _liquidacionRepositorio.GetAll()
                .Where(x => x.OrigenId == criterio.JurisdiccionId && x.DestinoId == criterio.JurisdiccionId);
            }
            else
            {
                query = _liquidacionRepositorio.GetAll()
                .Where(x => x.OrigenId == criterio.JurisdiccionId || x.DestinoId == criterio.JurisdiccionId);
            }

            query = query.Filter(criterio);

            var page = query.OrderBy(x => x.Fecha).ThenBy(x => x.Compensacion)
                .ToPage(criterio.Start, criterio.Length, r => r.MapListItem(User.GetJurisdiccionId(), jurisdiccionesEnte));

            return Ok(page);
        }

        [Route("{id:long}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _liquidacionRepositorio.Get(id);
            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var item = result.Map(User.GetJurisdiccionId(), jurisdiccionesEnte);
            return Ok(item);
        }

       
        [Route("{id:long}/RegistrarDeposito")]
        [HttpPut]
        public IHttpActionResult RegistrarDeposito(Int64 id)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var liquidacion = _liquidacionRepositorio.Get(id);
            var model = liquidacion.Map(User.GetJurisdiccionId(), jurisdiccionesEnte);

            model.FechaDeposito = HttpContext.Current.Request.Form.Get("FechaDeposito").Equals("null") ? null : HttpContext.Current.Request.Form.Get("FechaDeposito");
            model.Transaccion = HttpContext.Current.Request.Form.Get("Transaccion").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Transaccion");
            model.Entidad = HttpContext.Current.Request.Form.Get("Entidad").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Entidad");
            model.Sucursal = HttpContext.Current.Request.Form.Get("Sucursal").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Sucursal");
            model.Cajero = HttpContext.Current.Request.Form.Get("Cajero").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Cajero");

            if (!HttpContext.Current.Request.Form.Get("CuentaBancariaId").Equals("null"))
            {
                model.CuentaBancariaId = long.Parse(HttpContext.Current.Request.Form.Get("CuentaBancariaId"));
            }

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Deposito))
                return BadRequest(ModelState);

            model.MapDeposito(liquidacion);

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                if (liquidacion.Deposito.Recibo == null)
                {
                    liquidacion.Deposito.Recibo = new DocumentoDeposito();
                }
                liquidacion.Deposito.Recibo.Archivo = HttpContext.Current.Request.Files["file"].ToBytes();
            }

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}/RegistrarAcreditacion")]
        [HttpPut]
        public IHttpActionResult RegistrarAcreditacion(Int64 id)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var liquidacion = _liquidacionRepositorio.Get(id);
            var model = liquidacion.Map(User.GetJurisdiccionId(), jurisdiccionesEnte);

            model.ConciliacionVerificada = HttpContext.Current.Request.Form.Get("ConciliacionVerificada").Equals("null") ? null : HttpContext.Current.Request.Form.Get("ConciliacionVerificada");
            model.ConciliacionObservacion = HttpContext.Current.Request.Form.Get("ConciliacionObservacion").Equals("null") ? null : HttpContext.Current.Request.Form.Get("ConciliacionObservacion");

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Acreditacion))
                return BadRequest(ModelState);

            model.MapAcreditacion(liquidacion);

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                if (liquidacion.Conciliacion.Recibo == null)
                {
                    liquidacion.Conciliacion.Recibo = new DocumentoConciliacion();
                }
                liquidacion.Conciliacion.Recibo.Archivo = HttpContext.Current.Request.Files["file"].ToBytes();
            }

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}/ReciboDeposito")]
        [HttpGet]
        public IHttpActionResult ReciboDeposito(int id)
        {
            var liquidacion = _liquidacionRepositorio.Get(id);

            if (liquidacion == null || liquidacion.Deposito == null || liquidacion.Deposito.Recibo == null || liquidacion.Deposito.Recibo.Archivo == null)
                return NotFound();

            return new ResponseMessageResult(liquidacion.Deposito.Recibo.Archivo.ToPdfResponseMessage("ReciboDeposito"));
        }

        [Route("{id:long}/ComprobanteAcreditacion")]
        [HttpGet]
        public IHttpActionResult ComprobanteAcreditacion(int id)
        {
            var liquidacion = _liquidacionRepositorio.Get(id);

            if (liquidacion == null || liquidacion.Conciliacion == null || liquidacion.Conciliacion.Recibo == null || liquidacion.Conciliacion.Recibo.Archivo == null)
                return NotFound();

            return new ResponseMessageResult(liquidacion.Conciliacion.Recibo.Archivo.ToPdfResponseMessage("ReciboDeposito"));
        }


        //COMPENSACION
        // de las liquidaciones con tipo de movimiento = Acreditacion, saco la jurisdiccion destino.
        //la liquidacion origen debe coincidir con la jurisdiccion del usuario
        [Route("RegistrarCompensacion")]
        [HttpPut]
        public IHttpActionResult RegistrarCompensacion()
        {
            //Subir Archivos
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var model = new CompensacionDto { 
                EnteId = _jurisdiccionRepositorio.GetAll().Single(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).Id
            };

            model.LiquidacionesIds = HttpContext.Current.Request.Form.Get("liquidacionesArray").Equals("null") ? null : 
                                      (HttpContext.Current.Request.Form.Get("liquidacionesArray").Split(',')).Select(x => Convert.ToInt64(x)).ToList();
            model.FechaDeposito = HttpContext.Current.Request.Form.Get("FechaDeposito").Equals("null") ? null : HttpContext.Current.Request.Form.Get("FechaDeposito");
            model.Transaccion = HttpContext.Current.Request.Form.Get("Transaccion").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Transaccion");
            model.Entidad = HttpContext.Current.Request.Form.Get("Entidad").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Entidad");
            model.Sucursal = HttpContext.Current.Request.Form.Get("Sucursal").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Sucursal");
            model.Cajero = HttpContext.Current.Request.Form.Get("Cajero").Equals("null") ? null : HttpContext.Current.Request.Form.Get("Cajero");
            model.JurisdiccionDestino = long.Parse(HttpContext.Current.Request.Form.Get("jurisdiccionId"));
            if (!HttpContext.Current.Request.Form.Get("CuentaBancariaId").Equals("null"))
            {
                model.CuentaBancariaId = long.Parse(HttpContext.Current.Request.Form.Get("CuentaBancariaId"));
            }
            
            if (!_compensacionValidator.IsValid(model, ModelState))
                return BadRequest(ModelState);
         
            var liquidaciones = _liquidacionRepositorio.GetAll().Where(x => model.LiquidacionesIds.Contains(x.Id)).ToList();
            var compensacion = new Compensacion();

            model.Map(compensacion, liquidaciones);

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                if (compensacion.Deposito.Recibo == null)
                {
                    compensacion.Deposito.Recibo = new DocumentoDeposito();
                }
                compensacion.Deposito.Recibo.Archivo = HttpContext.Current.Request.Files["file"].ToBytes();
            }

            _compensacionRepositorio.Add(compensacion);
            _unitOfWork.SaveChanges();
            
            return Ok();
        }

        [Route("Report/{id:long}")]
        [HttpGet]
        public async Task<IHttpActionResult> Report(long id)
        {
            var liquidacion = _liquidacionRepositorio.Get(id);
            if (liquidacion == null)
                return NotFound();

            return await GetReportLiquidacionDeposito(liquidacion.Id, User.GetJurisdiccionId());
        }

        [Route("ReportData/{id:long}/{jurisdiccionId:long}")]
        [RequireGuidAuthentication]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult ReportData(int id, long jurisdiccionId)
        {
            var result = _liquidacionRepositorio.Get(id);
            if (result == null)
                return NotFound();

            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var item = result.Map(jurisdiccionId, jurisdiccionesEnte);

            return Ok(item);
        }
    }
}
