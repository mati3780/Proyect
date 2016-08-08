using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/TipoCuentaBancarias")]
    public class TipoCuentaBancariaController : ApiController
    {
        private readonly IRepositorio<TipoCuentaBancaria> _tipoCuentaBancariaRepositorio;

        public TipoCuentaBancariaController(IRepositorio<TipoCuentaBancaria> tipoCuentaBancariaRepositorio)
        {
            _tipoCuentaBancariaRepositorio = tipoCuentaBancariaRepositorio;
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var results = _tipoCuentaBancariaRepositorio.GetAll().OrderBy(i=> i.Orden).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }
    }
}