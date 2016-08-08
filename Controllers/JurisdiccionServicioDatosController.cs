using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/JurisdiccionServicioDatos")]
    public class JurisdiccionServicioDatosController : ApiController
    {
        #region Constructor

        private readonly IRepositorio<TramiteDato> _tramiteDatoRepositorio;
        private readonly IServicioRepositorio _servicioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TramiteDatoListDto> _validator;

        public JurisdiccionServicioDatosController(IUnitOfWork unitOfWork, IRepositorio<TramiteDato> tramiteDatoRepositorio,
													IServicioRepositorio servicioRepositorio, IValidator<TramiteDatoListDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tramiteDatoRepositorio = tramiteDatoRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _validator = validator;
        }

        #endregion

		[Route("")]
		[HttpGet]
	    public IHttpActionResult Get()
	    {
		    if (User.IsAdmin())
			    return Ok();

		    var jurisdiccionId = User.GetJurisdiccionId();

			var tiposTramites = _servicioRepositorio.GetAll().Where(tt => tt.Datos.Any(d => d.JurisdiccionId == jurisdiccionId))
																.ToList().Select(tt => tt.MapSimple()).ToList();

			return Ok(tiposTramites);
	    }

        [Route("{servicioId:long}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get(Int64 servicioId)
        {
            var servicio = _servicioRepositorio.Get(servicioId);
            var jurisdiccionId = User.GetJurisdiccionId();

            var definidos = servicio.Datos.Where(x => x.Jurisdiccion.Id == jurisdiccionId).ToList();

            var idsDefinidos = definidos.Select(r => r.TramiteDato.Id).ToList();
            var tipoEntidades = servicio.GetEntidades();
            var datos = _tramiteDatoRepositorio.GetAll().Where(x => !idsDefinidos.Contains(x.Id) && tipoEntidades.Contains(x.Entidad)).ToList();

            var datosRel = definidos.Map(servicio);

            if (datos.Any())
                datosRel.Merge(datos);

            return Ok(datosRel);
        }

		[Route("Configuracion/{servicioId:long}/{jurisdiccionesList}")]
		[HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Configuracion(long servicioId, string jurisdiccionesList)
		{
			List<JurisdiccionServicioDato> list;

			var servicio = _servicioRepositorio.GetConfiguracion(servicioId, jurisdiccionesList.Split(',').Select(long.Parse).ToArray(), out list);

			var result = list.Map(servicio);

			return Ok(result);
		}

	    [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]TramiteDatoListDto model)
        {
            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            var servicio = _servicioRepositorio.Get(model.ServicioId);
            var servicioEntidades = servicio.GetEntidades();
            
            var datosTramite = _tramiteDatoRepositorio.GetAll().Where(x => servicioEntidades.Contains(x.Entidad)).Select(r=>r.Id).ToList();

            foreach (var dato in model.Map())
            {
                if (datosTramite.Contains(dato.TramiteDatoId))
                    servicio.AddDato(dato);
            }
            
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]TramiteDatoListDto model)
        {
            var jurisdiccionId = User.GetJurisdiccionId();
            var servicio = _servicioRepositorio.Get(model.ServicioId);
            var servicioEntidades = servicio.GetEntidades();

            var definidos = servicio.Datos.Where(x => x.Jurisdiccion.Id == jurisdiccionId).ToList();

            if (!definidos.Any())
                return NotFound();

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            model.Map(definidos);

            var datosTramite = _tramiteDatoRepositorio.GetAll().Where(x => servicioEntidades.Contains(x.Entidad)).Select(r => r.Id).ToList();
            foreach (var dato in model.GetList().Where(datoDto => definidos.All(x => x.TramiteDato.Id != datoDto.Id)).ToList())
            {
                if (datosTramite.Contains(dato.Id))
                    servicio.AddDato(dato.Map());
            }

            _unitOfWork.SaveChanges();

            return Ok();
        }

    }
}