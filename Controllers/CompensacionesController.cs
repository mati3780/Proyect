using System.Linq;
using System.Web.Http;
using Helper.Repositorios.Contracts.Common;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.RuleFilter;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/Compensaciones")]
    public class CompensacionesController : ReportController
    {
        #region Constructor
        
        private readonly IRepositorio<Compensacion> _compensacionRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public CompensacionesController(IUnitOfWork unitOfWork, IRepositorio<Compensacion> compensacionRepositorio, IRepositorio<Jurisdiccion> jurisdiccionRepositorio)
        {
            _unitOfWork = unitOfWork;
            _compensacionRepositorio = compensacionRepositorio;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
        }

        #endregion

        [Route("Search")]
        [HttpPost]
        public IHttpActionResult Search(SearchCompensacionesDto criterio)
        {
            var query = _compensacionRepositorio.GetAll();

            if (User.IsAdmin() || User.IsEnte())
            {
                return Ok(query.GetEnte(criterio));
            }

            return Ok(query.GetJurisdiccion(criterio));
        }

        [Route("{id:long}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
             var compensacion = _compensacionRepositorio.Get(id);

            if (compensacion == null)
                return NotFound();
            
            return Ok(compensacion.Map());
        }

        [Route("{id:long}/Detalles")]
        [HttpGet]
        public IHttpActionResult GetDetalles(int id)
        {
            var jurisdiccionesEnte = _jurisdiccionRepositorio.GetAll().Where(x => x.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ENTE).ToList();
            var compensacion = _compensacionRepositorio.Get(id);

            if (compensacion == null)
                return NotFound();

            return Ok(compensacion.Liquidaciones.Select(x => x.MapResumen(jurisdiccionesEnte)).ToList());
        }
    }
}
