using System;
using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Servicios")]
    public class ServiciosController : ApiController
    {
        private readonly IRepositorio<Servicio> _servicioRepositorio;

        public ServiciosController(IRepositorio<Servicio> servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }
		
        [Route("")]
		[AllowAnonymous]
        public IHttpActionResult Get()
        {
            var results = _servicioRepositorio.GetAll().OrderBy(x => x.Orden).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }

		[Route("NoConfigurados"), HttpGet]
		public IHttpActionResult NoConfigurados()
		{
			if (User.IsAdmin())
				return Ok();

			var jurisdiccionId = User.GetJurisdiccionId();
			var tiposTramites = _servicioRepositorio.GetAll().Where(tt => tt.Datos.All(x => x.JurisdiccionId != jurisdiccionId)).ToList();

			return Ok(tiposTramites.Select(x => x.MapSimple()).ToList());
		}

		[Route("Disponibles")]
		[HttpGet]
	    public IHttpActionResult Disponibles()
	    {
			if (User.IsAdmin())
				return Ok();

			var jurisdiccionId = User.GetJurisdiccionId();
			var tiposTramites = _servicioRepositorio.GetAll()
														.Where(x => x.Datos.Any(r => r.JurisdiccionId == jurisdiccionId)).OrderBy(x => x.Descripcion)
														.ToList().Select(x => x.MapSimple()).ToList();
			return Ok(tiposTramites);
		}

		[Route("Condiciones")]
		[HttpGet]
	    public IHttpActionResult Condiciones()
	    {
			return Ok(Enum.GetValues(typeof (TramiteDatoCondicion))
							.Cast<int>()
							.Select(e => new
							             {
								             Key = e,
											 Value = Enum.GetName(typeof(TramiteDatoCondicion), e)
							}).ToList());
	    }
	}
}
