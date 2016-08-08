using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Profesiones")]
    public class ProfesionesController : ApiController
    {
        private readonly IRepositorio<Profesion> _profesionRepositorio;

        public ProfesionesController(IRepositorio<Profesion> profesionRepositorio)
        {
            _profesionRepositorio = profesionRepositorio;
        }
		
        [Route("")]
		[AllowAnonymous]
        public IHttpActionResult Get()
        {
            var results = _profesionRepositorio.GetAll().ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}
