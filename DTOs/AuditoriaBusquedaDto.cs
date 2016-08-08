using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROYECT.WebAPI.DTOs
{
	public class AuditoriaBusquedaDto
	{
		public string FechaOperacionInicial { get; set; }
		public string FechaOperacionFinal { get; set; }
		public string Usuario { get; set; }
		public string Ip { get; set; }
		public string Url { get; set; }
		public string Navegador { get; set; }
		public string Accion { get; set; }
		public string EntidadNombre { get; set; }
		public string EntidadKey { get; set; }
		public string JsonOperacion { get; set; }
		public int Start { get; set; }
		public int Length { get; set; }
	}
}
