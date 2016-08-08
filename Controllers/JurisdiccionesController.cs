using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using FluentValidation;
using LinqKit;
using Helper.Dominio.Model.Entities;
using Helper.Repositorios.Contracts.Common;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.RuleFilter;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Jurisdicciones")]
    public class JurisdiccionesController : ApiController
    {
        #region Constructor

        private readonly IJurisdiccionRepositorio _jurisdiccionRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<JurisdiccionDto> _validator;

        public JurisdiccionesController(IUnitOfWork unitOfWork, IJurisdiccionRepositorio jurisdiccionRepositorio, IValidator<JurisdiccionDto> validator)
        {
            _unitOfWork = unitOfWork;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _validator = validator;
        }

        #endregion

        [HttpPost]
        [Route("Search")]
        [AllowAnonymous]
        public IHttpActionResult Search([FromBody]JurisdiccionBusquedaDto criterio)
        {
            var query = _jurisdiccionRepositorio.GetAll();

            if (User.IsAdmin())
            {
                query = query.Filter(criterio);

                var page = query.OrderByDescending(x => x.Orden.HasValue).ThenBy(x => x.Orden)
                                .ThenBy(x => x.Descripcion).ToPage(criterio.Start, criterio.Length, j => j.Map());

                return Ok(page);
            }

            var jurisdiccionId = User.GetJurisdiccionId();
            var jurisdicciones = query.Where(x => x.Id == jurisdiccionId).ToPage(criterio.Start, criterio.Length, j => j.Map());

            return Ok(jurisdicciones);
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            if (User.IsAdmin() || User.IsEnte())
                return Ok(_jurisdiccionRepositorio.GetAll()
													.OrderByDescending(x => x.Orden.HasValue)
													.ThenBy(x => x.Orden)
													.ThenBy(x => x.Descripcion)
													.ToList()
													.Select(x => x.MapSimple()));

            var jurisdiccionId = User.GetJurisdiccionId();
            return Ok(_jurisdiccionRepositorio.GetAll()
												.Where(x => x.Id == jurisdiccionId)
												.ToList()
												.Select(x => x.MapSimple()));
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            if (!_validator.IsValid(new JurisdiccionDto { Id = id }, ModelState, RuleSetValidation.Obtener))
                return new StatusCodeResult(HttpStatusCode.Forbidden, this);

            var result = _jurisdiccionRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }

        [Route("Requirentes"),
        HttpGet,
        AllowAnonymous]
        public IHttpActionResult GetRequirentes()
        {
            var results = _jurisdiccionRepositorio.GetRequirientes();
            return Ok(results.ToList().Select(x => x.MapSimple()));
        }

        [Route("Informantes/{servicioId}/{jurisdiccionSolicitanteId}/{jurisdiccionesIdAExcluir?}"),
        HttpGet,
        AllowAnonymous]
        public IHttpActionResult GetInformantes(long servicioId, long jurisdiccionSolicitanteId, string jurisdiccionesIdAExcluir = null)
        {
            var informantes = _jurisdiccionRepositorio.GetInformantes(servicioId, jurisdiccionSolicitanteId, jurisdiccionesIdAExcluir);
            return Ok(informantes.Select(x => x.MapSimple()));
        }

        [Route("Solicitantes")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetSolicitantes()
        {
            var result = _jurisdiccionRepositorio.GetAll()
                                                .Where(j => j.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ReparticionRequirente)
                                                .ToList();
            return Ok(result.Select(j => new
            {
                j.Id,
                j.Descripcion,
                j.Sigla
            }));
        }

        [Route("{id:long}/Feriados")]
        public IHttpActionResult GetFeriados(Int64 id)
        {
            var result = _jurisdiccionRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Feriados.Select(x => x.Map()).ToList());
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]JurisdiccionDto model)
        {
            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Obtener))
                return BadRequest(ModelState);

            if (!_validator.IsValidWithDefault(model, ModelState, (User.IsAdmin() ? RuleSetValidation.Admin : RuleSetValidation.Usuario)))
                return BadRequest(ModelState);

            _jurisdiccionRepositorio.Add(model.Map(new Jurisdiccion()));
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]JurisdiccionDto model)
        {
            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Obtener))
                return BadRequest(ModelState);

            var item = _jurisdiccionRepositorio.Get(model.Id);

            if (item == null)
                return NotFound();

            model.Id = id;

            if (!_validator.IsValidWithDefault(model, ModelState, (User.IsAdmin() ? RuleSetValidation.Admin : RuleSetValidation.Usuario)))
                return BadRequest(ModelState);

            model.Map(item);

            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("Descripcion")]
        [HttpGet]
        public IHttpActionResult GetDenominacion()
        {
            var jurisdiccionId = User.GetJurisdiccionId();
            return Ok(new { Descripcion = _jurisdiccionRepositorio.GetAll().Single(x => x.Id == jurisdiccionId).Descripcion });
        }

        [Route("Bloqueadas")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Bloqueadas()
        {
            var result = _jurisdiccionRepositorio.GetAll()
                                                .Where(j => j.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.BloqueadoRequiriente)
                                                .ToList();
            return Ok(result.Select(j => j.MapSimple()));
        }

        //[Route("{id:long}")]
        //[HttpDelete]
        //public IHttpActionResult Delete(int id)
        //{
        //    var item = _jurisdiccionRepositorio.Get(id);
        //    if (item == null)
        //        return NotFound();
        //    _jurisdiccionRepositorio.Delete(item);
        //    _unitOfWork.SaveChanges();
        //    return Ok();
        //}
    }
}
