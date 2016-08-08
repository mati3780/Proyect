using System;
using PROYECT.Dominio.Enums;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteDatoDto
    {
        public Int64 Id { get; set; }
		public String Nombre { get; set; }
        public String Label { get; set; }
        public TramiteDatoCondicion? Condicion { get; set; }
		public string CondicionDescripcion { get; set; }
    }
}
