using System;
using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Attributes;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Enums;
using FluentValidation;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/CuentaBancarias")]
    public class CuentasBancariasController : ApiController
    {
        private readonly IRepositorio<CuentaBancaria> _cuentabancariaRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CuentaBancariaDto> _validator;

        public CuentasBancariasController(IUnitOfWork unitOfWork, IRepositorio<CuentaBancaria> cuentabancariaRepositorio, IValidator<CuentaBancariaDto> validator)
        {
            _unitOfWork = unitOfWork;
            _cuentabancariaRepositorio = cuentabancariaRepositorio;
            _validator = validator;
        }
        
        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var jurisdiccionId = User.GetJurisdiccionId();
            var jurisdicciones = _cuentabancariaRepositorio.GetAll().Where(x => x.Jurisdiccion.Id == jurisdiccionId && (!x.FechaVigencia.HasValue || x.FechaVigencia >= DateTime.Today)).ToPage(start, length, j => j.Map(), j => j.Id);
            return Ok(jurisdicciones);
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            var result = _cuentabancariaRepositorio.Get(id);

            if (result == null)
                return NotFound();

            if (!_validator.IsValid(new CuentaBancariaDto { Id = id }, ModelState, RuleSetValidation.Obtener))
                return BadRequest(ModelState);

            return Ok(result.Map());
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        public IHttpActionResult Post([FromBody]CuentaBancariaDto model)
        {
            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            _cuentabancariaRepositorio.Add(model.Map(new CuentaBancaria()));
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [Route("{id:long}")]
        [HttpPut]
        [ValidateModel]
        public IHttpActionResult Put(Int64 id, [FromBody]CuentaBancariaDto model)
        {
            var item = _cuentabancariaRepositorio.Get(model.Id);

            if (item == null)
                return NotFound();

            model.Id = id;

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            model.MapSimple(item);
            _unitOfWork.SaveChanges();

            return Ok();
        }

    }
}
