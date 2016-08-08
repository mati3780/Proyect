using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PROYECT.WebAPI.DTOs
{
	public class AuditoriaOperacionGridDTO
	{
		public long Id { get; set; }
		public string FechaOperacion { get; set; }
		public string Usuario { get; set; }
		public string Ip { get; set; }
		public string Url { get; set; }
		public string Navegador { get; set; }
	}

	public class AuditoriaOperacionDTO
	{
		public long Id { get; set; }
		public string FechaOperacion { get; set; }
		public string Usuario { get; set; }
		public string Ip { get; set; }
		public string Url { get; set; }
		public string Navegador { get; set; }
		public List<AuditoriaOperacionDetalleDTO> Detalles { get; set; }
	}

	public class AuditoriaOperacionDetalleDTO
	{
		public string EntidadNombre { get; set; }
		public Dictionary<string, string> EntidadKey { get; set; }
		public string Accion { get; set; }
		public List<Operacion> JsonOperacion { get; set; }
	}

	public class Operacion
	{
		public string Nombre { get; set; }
		public string Value { get; set; }
		public string OldValue { get; set; }
	}
}
