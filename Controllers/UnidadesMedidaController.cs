using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/UnidadesMedida")]
    public class UnidadesMedidaController : ApiController
    {
        private readonly IRepositorio<UnidadMedida> _tipoUnidadMedidaRepositorio;

        public UnidadesMedidaController(IRepositorio<UnidadMedida> tipoUnidadMedidaRepositorio)
        {
            _tipoUnidadMedidaRepositorio = tipoUnidadMedidaRepositorio;
        }


        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var results = _tipoUnidadMedidaRepositorio.GetAll().OrderBy(x => x.Id).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}
