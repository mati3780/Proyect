using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.Dominio.Model.Entities;
using Newtonsoft.Json.Linq;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
	public static class AuditoriaExtensions
	{
		public static AuditoriaOperacionGridDTO MapForGrid(this AuditoriaOperacion value)
		{
			if (value == null)
				return null;

			var model = new AuditoriaOperacionGridDTO
			            {
				            Id = value.Id,
							FechaOperacion = value.FechaOperacion.ToString("dd/MM/yyyy HH:mm:ss"),
							Ip = value.Ip,
							Navegador = value.Navegador,
							Url = value.Url,
							Usuario = value.Usuario
			            };
			return model;
		}

		public static AuditoriaOperacionDTO Map(this AuditoriaOperacion value)
		{
			if (value == null)
				return null;

			var model = new AuditoriaOperacionDTO
			            {
				            Id = value.Id,
				            FechaOperacion = value.FechaOperacion.ToString("dd/MM/yyyy HH:mm:ss"),
				            Ip = value.Ip,
				            Navegador = value.Navegador,
				            Url = value.Url,
				            Usuario = value.Usuario,
							Detalles = value.AuditoriaOperacionDetalles.Select(x => x.MapDetalle()).ToList()
			            };

			return model;
		}

		public static AuditoriaOperacionDetalleDTO MapDetalle(this AuditoriaOperacionDetalle value)
		{
			if (value == null)
				return null;

			var model = new AuditoriaOperacionDetalleDTO
			            {
							Accion = value.Accion,
							EntidadNombre = value.EntidadNombre,
							EntidadKey = JObject.Parse(value.EntidadKey)["keys"].ToDictionary(key => key.Value<string>("Key"), key => key.Value<string>("Value")),
							JsonOperacion = JObject.Parse(value.JsonOperacion)["Columns"]
																				.Select(columna => new Operacion
																				{
																					Nombre = columna.Value<string>("name"),
																					Value = columna.Value<string>("Value"),
																					OldValue = columna.Value<string>("OldValue")
																				}).ToList()
						};
			return model;
		}
	}
}
