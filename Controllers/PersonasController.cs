using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Personas")]
    public class PersonasController : ApiController
    {
        private readonly IRepositorio<Persona> _personaRepositorio;

        public PersonasController(IRepositorio<Persona> personaRepositorio)
        {
            _personaRepositorio = personaRepositorio;
        }

        [Route("")]
        public IHttpActionResult Get(int start, int length)
        {
            var jurisdicciones = _personaRepositorio.GetAll().ToPage(start, length, j => j.Map(), j => j.Id);
            return Ok(jurisdicciones);
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            var result = _personaRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }
    }
}
