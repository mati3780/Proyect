using System;
using System.Linq;
using System.Web.Http;
using FluentValidation;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Feriados")]
    public class FeriadosController : ApiController
    {
        #region Contructor

        private readonly IRepositorio<Feriado> _feriadoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<FeriadoDto> _validator;

        public FeriadosController(IUnitOfWork unitOfWork, IRepositorio<Feriado> feriadoRepositorio, IValidator<FeriadoDto> validator)
        {
            _unitOfWork = unitOfWork;
            _feriadoRepositorio = feriadoRepositorio;
            _validator = validator;
        }

        #endregion

        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var jurisdicciones = _feriadoRepositorio.GetAll().ToPage(start, length, x => x.Map(), x => x.Descripcion);
            return Ok(jurisdicciones);
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            var result = _feriadoRepositorio.Get(id);

            if (result == null)
                return NotFound();

            if (!_validator.IsValid(new FeriadoDto { Id = id }, ModelState, RuleSetValidation.Obtener))
                return BadRequest(ModelState);
            
            return Ok(result.Map());
        }

        [Route("Search/{nacionales}")]
        [HttpGet]
        public IHttpActionResult Search(int start, int length, Boolean nacionales)
        {
            var query = _feriadoRepositorio.GetAll().Where(x => x.Fecha.Year == DateTime.Today.Year);

            if (nacionales)
                query = query.Where(x => x.Jurisdiccion == null);
            else if (User.IsAdmin())
	            query = query.Where(x => x.Jurisdiccion != null);
            else
            {
	            var jurisdiccionId = User.GetJurisdiccionId();
				query = query.Where(x => x.Jurisdiccion.Id == jurisdiccionId);
            }
            
            var jurisdicciones = query.ToPage(start, length, x => x.Map(), x => x.Fecha);

            return Ok(jurisdicciones);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]FeriadoDto model)
        {
            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            _feriadoRepositorio.Add(model.Map(new Feriado()));
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        public IHttpActionResult Put(Int64 id, [FromBody]FeriadoDto model)
        {
            var item = _feriadoRepositorio.Get(id);

            if (item == null)
                return NotFound();

            model.Id = id;

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            model.Map(item);
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
		{
			var item = _feriadoRepositorio.Get(id);

			if (item == null)
				return NotFound();

            if (!_validator.IsValid(new FeriadoDto { Id = id }, ModelState, RuleSetValidation.Delete))
                return BadRequest(ModelState);
            
            _feriadoRepositorio.Delete(item);
            _unitOfWork.SaveChanges();

            return Ok();
        }
    }
}
