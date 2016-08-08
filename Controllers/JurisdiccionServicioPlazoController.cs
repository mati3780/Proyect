using System;
using System.Linq;
using System.Web.Http;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/JurisdiccionServicioPlazos")]
    public class JurisdiccionServicioPlazoController : ApiController
    {
        #region Constructor

        private readonly IRepositorio<JurisdiccionServicioPlazo> _jurisdiccionServicioPlazoRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IRepositorio<Plazo> _plazoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<JurisdiccionServicioPlazoDto> _validator;

        public JurisdiccionServicioPlazoController(IUnitOfWork unitOfWork, IRepositorio<JurisdiccionServicioPlazo> jurisdiccionServicioPlazoRepositorio, 
														IRepositorio<Plazo> plazoRepositorio, IRepositorio<Jurisdiccion> jurisdiccionRepositorio, 
                                                        IValidator<JurisdiccionServicioPlazoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _jurisdiccionServicioPlazoRepositorio = jurisdiccionServicioPlazoRepositorio;
            _plazoRepositorio = plazoRepositorio;
            _validator = validator;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
        }

        #endregion

        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var jurisdiccionId = User.GetJurisdiccionId();
            var items = _jurisdiccionServicioPlazoRepositorio.GetAll()
																	.Where(x=> x.JurisdiccionId == jurisdiccionId)
																	.OrderBy(x => x.Servicio.Descripcion)
																	.ToPage(start, length, j => j.Map());
            return Ok(items);
        }

        [Route("{servicioId:long}/PlazosNoConfigurados")]
		[HttpGet]
        public IHttpActionResult PlazosNoConfigurados(long servicioId)
        {
            var jurisdiccionId = User.GetJurisdiccionId();
            var list = _jurisdiccionServicioPlazoRepositorio.GetAll()
                .Where(x => x.JurisdiccionId == jurisdiccionId && x.ServicioId == servicioId && !x.VigenciaHasta.HasValue)
                .Select(x => x.Plazo.Id).ToList();
            
            var result = _plazoRepositorio.GetAll().Where(x=> !list.Contains(x.Id)).OrderBy(x => x.Descripcion).ToList();

            return Ok(result.ToList().Select(x=> x.MapSimple()));
        }

		[Route("{servicioId:long}/PlazosDisponibles")]
		[HttpGet]
	    public IHttpActionResult PlazosDisponibles(long servicioId)
		{
			if (User.IsAdmin())
				return Ok();

			var jurisdiccionId = User.GetJurisdiccionId();

			var result = _jurisdiccionServicioPlazoRepositorio
									.GetAll()
									.Where(x => x.ServicioId == servicioId && x.JurisdiccionId == jurisdiccionId &&
												((!x.VigenciaHasta.HasValue && DateTime.Now >= x.VigenciaDesde) || 
												 (x.VigenciaHasta.HasValue && DateTime.Now <= x.VigenciaHasta.Value)))
									.Select(x => x.Plazo)
									.OrderBy(x => x.Descripcion)
									.ToList()
									.Select(p => p.Map());
			return Ok(result);
		}

		[Route("{servicioId:long}/Disponible/{plazoId:long}")]
		[HttpGet]
	    public IHttpActionResult Disponible(long servicioId, long plazoId)
		{
			if (User.IsAdmin())
				return Ok();

			var jurisdiccionId = User.GetJurisdiccionId();

            var result = _jurisdiccionServicioPlazoRepositorio
									.GetAll()
									.SingleOrDefault(x => x.ServicioId == servicioId && x.PlazoId == plazoId && x.JurisdiccionId == jurisdiccionId &&
															((!x.VigenciaHasta.HasValue && DateTime.Now >= x.VigenciaDesde) ||
															(x.VigenciaHasta.HasValue && DateTime.Now <= x.VigenciaHasta.Value))).Map();
			return Ok(result);
	    }


		/// <summary>
		/// Devuelve lista de JurisdiccionServicioPlazo que se encuentran disponibles a la fecha actual para el ServicioId y JurisdiccionId especificados
		/// </summary>
		/// <param name="servicioId"></param>
		/// <param name="jurisdiccionId"></param>
		/// <returns></returns>
		[Route("{servicioId:long}/DisponiblesPublico/{jurisdiccionId:long}/{reparticionId:long?}")]
		[HttpGet]
		[AllowAnonymous]
	    public IHttpActionResult DisponiblesPublico(long servicioId, long jurisdiccionId, long? reparticionId = null)
		{
            var result = _jurisdiccionServicioPlazoRepositorio.GetAll()
															  .Where(x => x.ServicioId == servicioId && x.JurisdiccionId == jurisdiccionId &&
															  (!x.VigenciaHasta.HasValue || (x.VigenciaDesde <= DateTime.Now && x.VigenciaHasta.Value >= DateTime.Now)))
															  .ToList().Select(x => x.Map((reparticionId == null ? null : _jurisdiccionRepositorio.Get((long)reparticionId))));
			return Ok(result);
	    }

        [Route("{id:long}")]
        public IHttpActionResult Get(long id)
        {
            if (!_validator.IsValid(new JurisdiccionServicioPlazoDto { Id = id }, ModelState, RuleSetValidation.Obtener))
                return BadRequest(ModelState);

            var result = _jurisdiccionServicioPlazoRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]JurisdiccionServicioPlazoDto model)
        {
            if (!_validator.IsValid(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            _jurisdiccionServicioPlazoRepositorio.Add(model.Map(new JurisdiccionServicioPlazo()));
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]JurisdiccionServicioPlazoDto model)
        {
            var item = _jurisdiccionServicioPlazoRepositorio.Get(model.Id);

            if (item == null)
                return NotFound();

            model.Id = id;
            model.EntidadVigenciaHasta = item.VigenciaHasta;
            model.EntidadVigenciaDesde = item.VigenciaDesde;

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);
            
            item.VigenciaHasta = model.VigenciaHasta.ToNuleableDate();

            _unitOfWork.SaveChanges();

            return Ok();
        }
    }
}