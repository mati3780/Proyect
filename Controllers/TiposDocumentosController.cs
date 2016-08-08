using System.Linq;
using System.Web.Http;
using System.Web.Profile;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/TiposDocumentos")]
    public class TiposDocumentosController : ApiController
    {
        private readonly IRepositorio<TipoDocumento> _tipoDocumentoRepositorio;

        public TiposDocumentosController(IRepositorio<TipoDocumento> tipoDocumentoRepositorio)
        {
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;
        }

        [Route(""), AllowAnonymous]
        public IHttpActionResult Get(long id)
        {
            return Ok(_tipoDocumentoRepositorio.Get(id).Map());
        }

        [Route(""), AllowAnonymous]
        public IHttpActionResult Get()
        {
            var results = _tipoDocumentoRepositorio.GetAll().ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}
