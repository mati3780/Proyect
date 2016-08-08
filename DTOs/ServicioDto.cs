using System;

namespace PROYECT.WebAPI.DTOs
{
    public class ServicioDto : SeleccionableDto
    {
		public bool Inmueble { get; set; }
        public Int16? Orden { get; set; }
    }
}
