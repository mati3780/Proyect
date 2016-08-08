using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Controllers
{
    [RoutePrefix("api/TramiteEstado")]
    public class TramiteEstadoController : ApiController
    {
        private readonly IRepositorio<SubTipoEstadoTramite> _subTipoEstadoTramiteJurisdiccionRepositorio;
        private readonly IRepositorio<TipoEstadoTramite> _tipoEstadoTramiteJurisdiccionRepositorio;

        public TramiteEstadoController(IRepositorio<SubTipoEstadoTramite> subTipoEstadoTramiteJurisdiccionRepositorio,
                                       IRepositorio<TipoEstadoTramite> tipoEstadoTramiteJurisdiccionRepositorio)
        {
            _subTipoEstadoTramiteJurisdiccionRepositorio = subTipoEstadoTramiteJurisdiccionRepositorio;
            _tipoEstadoTramiteJurisdiccionRepositorio = tipoEstadoTramiteJurisdiccionRepositorio;
        }

        [Route("TipoEstados")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult TipoEstados()
        {
            var listaEstados = _tipoEstadoTramiteJurisdiccionRepositorio.GetAll().ToList().Select(x => x.Map()).ToList();
            return Ok(listaEstados);
        }

        [Route("TipoSubEstados/{tipoEstadoId:long}")]
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult TipoSubEstados(long tipoEstadoId)
        {
            var listaSubEstados =
                _subTipoEstadoTramiteJurisdiccionRepositorio.GetAll()
                    .Where(t => t.TipoEstado.Id == tipoEstadoId)
                    .OrderBy(e => e.Orden)
                    .ToList()
                    .Select(x => x.Map())
                    .ToList();
            return Ok(listaSubEstados);
        }
    }
}
