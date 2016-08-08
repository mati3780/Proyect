using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/JurisdiccionEstados")]
    public class JurisdiccionEstadosController : ApiController
    {
        private readonly IRepositorio<TipoEstadoTramite> _tipoEstadoJurisdiccionRepositorio;

        public JurisdiccionEstadosController(IRepositorio<TipoEstadoTramite> tipoEstadoJurisdiccionRepositorio)
        {
            _tipoEstadoJurisdiccionRepositorio = tipoEstadoJurisdiccionRepositorio;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var results = _tipoEstadoJurisdiccionRepositorio.GetAll().OrderBy(x => x.Orden).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}
