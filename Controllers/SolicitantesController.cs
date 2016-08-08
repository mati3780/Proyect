using System;
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
    [RoutePrefix("api/Solicitantes")]
    public class SolicitantesController : ApiController
    {
        #region Constructor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorio<Solicitante> _solicitanteRepositorio;
        private readonly IValidator<SolicitanteDto> _validator;

        public SolicitantesController(IUnitOfWork unitOfWork, IRepositorio<Solicitante> solicitanteRepositorio, IValidator<SolicitanteDto> validator)
        {
            _solicitanteRepositorio = solicitanteRepositorio;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        #endregion

        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var jurisdicciones = _solicitanteRepositorio.GetAll().ToPage(start, length, j => j.Map(), j => j.Id);
            return Ok(jurisdicciones);
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            var result = _solicitanteRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }

        [Route("{id:long}")]
        [HttpPut]
        public IHttpActionResult Put(Int64 id, [FromBody]SolicitanteDto model)
        {
            var item = _solicitanteRepositorio.Get(id);

            if (item == null)
                return NotFound();

            model.Id = id;

            if (!_validator.IsValid(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);
            
            model.Map(item);

            _unitOfWork.SaveChanges();

            return Ok();
        }
    }
}
