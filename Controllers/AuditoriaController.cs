using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Helper.Dominio.Model.Entities;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;
using PROYECT.WebAPI.Extensions.RuleFilter;

namespace PROYECT.WebAPI.Controllers
{
	[RoutePrefix("api/Auditoria")]
	public class AuditoriaController : ApiController
	{
		private readonly IRepositorio<AuditoriaOperacion> _auditoriaRepositorio;
		private readonly IRepositorio<AuditoriaOperacionDetalle> _auditoriaDetalleRepositorio;

		public AuditoriaController(IRepositorio<AuditoriaOperacion> auditoriaRepositorio, IRepositorio<AuditoriaOperacionDetalle> auditoriaDetalleRepositorio)
		{
			_auditoriaRepositorio = auditoriaRepositorio;
			_auditoriaDetalleRepositorio = auditoriaDetalleRepositorio;
		}

		// POST: api/Auditoria
		[HttpPost]
		[Route("")]
		public IHttpActionResult Post([FromBody]AuditoriaBusquedaDto criterio)
		{
			//Si todos los parametros son nulos, entonces devuelve resultado vacío
			if (string.IsNullOrWhiteSpace(criterio.FechaOperacionInicial) && string.IsNullOrWhiteSpace(criterio.FechaOperacionFinal) &&
				string.IsNullOrWhiteSpace(criterio.Usuario) && string.IsNullOrWhiteSpace(criterio.Ip) && string.IsNullOrWhiteSpace(criterio.Url) &&
				string.IsNullOrWhiteSpace(criterio.Navegador) && string.IsNullOrWhiteSpace(criterio.EntidadNombre) &&
				string.IsNullOrWhiteSpace(criterio.EntidadKey) && string.IsNullOrWhiteSpace(criterio.Accion) && string.IsNullOrWhiteSpace(criterio.JsonOperacion))
				return Ok(new List<AuditoriaOperacion>().ToPage(criterio.Start, criterio.Length, x => x.MapForGrid()));

			var auditorias = _auditoriaRepositorio
											.GetAll()
											.Filter(criterio)
											.OrderByDescending(x => x.FechaOperacion)
											.ToPage(criterio.Start, criterio.Length, x => x.MapForGrid());

			return Ok(auditorias);
		}

		// GET: api/Auditoria/5
		[Route("{id}")]
		[HttpGet]
		public IHttpActionResult Get(int id)
		{
			var auditoria = _auditoriaRepositorio.Get(id).Map();
			return Ok(auditoria);
		}

		[Route("Usuarios")]
		[HttpGet]
		public IHttpActionResult Usuarios()
		{
			var result = _auditoriaRepositorio.GetAll().Select(x => x.Usuario).Distinct().OrderBy(x => x).ToList();
			return Ok(result);
		}

		[Route("Urls")]
		[HttpGet]
		public IHttpActionResult Urls()
		{
			var result = _auditoriaRepositorio.GetAll().Select(x => x.Url).Distinct().OrderBy(x => x).ToList();
			return Ok(result);
		}

		[Route("Navegadores")]
		[HttpGet]
		public IHttpActionResult Navegadores()
		{
			var result = _auditoriaRepositorio.GetAll().Select(x => x.Navegador).Distinct().OrderBy(x => x).ToList();
			return Ok(result);
		}

		[Route("Entidades")]
		[HttpGet]
		public IHttpActionResult Entidades()
		{
			var result = _auditoriaDetalleRepositorio.GetAll().Select(x => x.EntidadNombre).Distinct().OrderBy(x => x).ToList();
			return Ok(result);
		}
	}
}
