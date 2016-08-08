using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/SolicitudesEstados")]
    public class SolicitudesEstadosController : ApiController
    {
        private readonly IRepositorio<TipoEstadoSolicitud> _tramiteEstadoRepositorio;

        public SolicitudesEstadosController(IRepositorio<TipoEstadoSolicitud> tramiteEstadoRepositorio)
        {
            _tramiteEstadoRepositorio = tramiteEstadoRepositorio;
        }


        [Route("")]
        public IHttpActionResult Get()
        {
            var results = _tramiteEstadoRepositorio.GetAll().OrderBy(x => x.Orden).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}
