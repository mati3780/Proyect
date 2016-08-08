using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/TipoEstadoAdhesiones")]
    public class TiposEstadosAdhesionesController : ApiController
    {
        private readonly IRepositorio<TipoEstadoAdhesion> _tipoEstadoAdhesionRepositorio;

        public TiposEstadosAdhesionesController(IRepositorio<TipoEstadoAdhesion> tipoEstadoAdhesionRepositorio)
        {
            _tipoEstadoAdhesionRepositorio = tipoEstadoAdhesionRepositorio;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var results = _tipoEstadoAdhesionRepositorio.GetAll().OrderBy(i=> i.Orden).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}