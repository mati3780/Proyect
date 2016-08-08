using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using Helper.Repositorios.Contracts.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Attributes;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Enums;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Helpers.Email;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/ContribucionProyect")]
    public class ContribucionesProyectController : ApiController
    {
        private readonly ITasaNacionalRepositorio _contribucionProyectRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ContribucionProyectDto> _validator;

        public ContribucionesProyectController(IUnitOfWork unitOfWork, ITasaNacionalRepositorio contribucionProyectRepositorio, IValidator<ContribucionProyectDto> validator,
                                                IRepositorio<Jurisdiccion> jurisdiccionRepositorio)
        {
            _unitOfWork = unitOfWork;
            _contribucionProyectRepositorio = contribucionProyectRepositorio;
            _validator = validator;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
        }

        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var tasas = _contribucionProyectRepositorio.GetAll().OrderByDescending(x => x.FechaDesde).ToPage(start, length, j => j.Map());
            return Ok(tasas);
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var result = _contribucionProyectRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }

        [Route("Actual/{reparticionId:long?}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Actual(long? reparticionId = null)
        {
            var jurisdiccion = reparticionId != null ? _jurisdiccionRepositorio.Get((long)reparticionId) : null;
            var result = _contribucionProyectRepositorio.GetVigenteCosto();

            if (jurisdiccion != null && jurisdiccion.NoRequiereTasaNacional)
                result = 0;

            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        public IHttpActionResult Post([FromBody]ContribucionProyectDto model)
        {
            model.FechaDesde = ObtenerFechaVigenciaDesde();

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.New))
                return BadRequest(ModelState);

            _contribucionProyectRepositorio.Add(model.Map(new TasaNacional()));
            _unitOfWork.SaveChanges();

            SendMail(TipoAccion.Alta, model.Valor, model.FechaDesde);

            return Ok();
        }

        [Route("{id:int}")]
        [HttpPut]
        [ValidateModel]
        public IHttpActionResult Put(Int64 id, [FromBody]ContribucionProyectDto model)
        {
            var item = _contribucionProyectRepositorio.Get(model.Id);

            if (item == null)
                return NotFound();

            model.Id = id;

            if (!_validator.IsValidWithDefault(model, ModelState, RuleSetValidation.Edit))
                return BadRequest(ModelState);

            model.MapSimple(item);
            _unitOfWork.SaveChanges();

            SendMail(TipoAccion.Baja, model.Valor, model.FechaHasta);

            return Ok();
        }

        [Route("ObtenerFechaDesde")]
        [HttpGet]
        public IHttpActionResult ObtenerFechaDesde()
        {
            return Ok(ObtenerFechaVigenciaDesde());
        }


        #region Metodos Privados

        private string ObtenerFechaVigenciaDesde()
        {
            if (_contribucionProyectRepositorio.GetAll().Any(x => x.FechaHasta == null))
                return string.Empty;

            var fechaDesde = _contribucionProyectRepositorio.GetAll().Where(x => x.FechaHasta != null).Max(x => x.FechaHasta);
            return fechaDesde?.AddDays(1).ToString("d") ?? DateTime.Today.AddDays(1).ToString("d");
        }

        private void SendMail(TipoAccion accion, decimal valor, string fechaVigencia)
        {
            var html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailActualizacionContribucion.html"))
                                           .Replace("[[Fecha]]", DateTime.Now.ToLongDateString())
                                           .Replace("[[TipoOperacion]]", accion.ToString())
                                           .Replace("[[Precio]]", valor.ToString())
                                           .Replace("[[FechaVigencia]]", $"Fecha Vigencia {(accion == TipoAccion.Alta ? "Desde" : "Hasta")}: {fechaVigencia}");

            MailSender.SendEmail(new[] { ConfigurationManager.AppSettings["MailSenderFrom"], ConfigurationManager.AppSettings["MailEnte"] },
                                 "PROYECT - Aviso de Actualización Contribución PROYECT",
                                 html,
                                 null);
        }

        #endregion

    }

    enum TipoAccion
    {
        Alta,
        Baja
    }
}
