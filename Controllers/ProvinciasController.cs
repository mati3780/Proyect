using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Provincias")]
    public class ProvinciasController : ApiController
    {
        private readonly IRepositorio<Provincia> _provinciaRepositorio;
        private readonly IRepositorio<Municipio> _municipioRepositorio;
        private readonly IRepositorio<Localidad> _localidadRepositorio;
        public ProvinciasController(IRepositorio<Provincia> provinciaRepositorio, IRepositorio<Municipio> municipioRepositorio, IRepositorio<Localidad> localidadRepositorio)
        {
            _provinciaRepositorio = provinciaRepositorio;
            _municipioRepositorio = municipioRepositorio;
            _localidadRepositorio = localidadRepositorio;
        }

        [Route(""), AllowAnonymous]
        public IHttpActionResult Get()
        {
            var results = _provinciaRepositorio.GetAll().ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }

        [Route("{id:long}")]
        public IHttpActionResult Get(long id)
        {
            var result = _provinciaRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Map());
        }
        
        [Route("{id:long}/Municipios"), HttpGet]
        public IHttpActionResult Municipios(long id)
        {
            var result = _provinciaRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Municipios.OrderBy(m => m.Descripcion).Select(x => x.Map()).ToList());
        }

        [Route("{id:long}/Localidades"), HttpGet, AllowAnonymous]
        public IHttpActionResult GetLocalidadesPorProvincia(long id)
        {
           var results = _localidadRepositorio.GetAll().Where(x => x.Municipio.Provincia.Id == id).OrderBy(l => l.Descripcion).ToList();
            return Ok(results.Select(x => x.Map()).ToList());
        }

        [Route("Municipios/{id:long}/Localidades"), HttpGet]
        public IHttpActionResult GetLocalidades(long id)
        {
            var result = _municipioRepositorio.Get(id);

            if (result == null)
                return NotFound();

            return Ok(result.Localidades.OrderBy(l => l.Descripcion).Select(x => x.Map()).ToList());
        }
    }
}
