using System;
using System.Collections.Generic;

namespace PROYECT.WebAPI.DTOs
{
    public class TipoEstadoJurisdiccionDto
    {
        public Int64 Id { get; set; }
        public String Descripcion { get; set; }
        public Int16? Orden { get; set; }
        public List<SubTipoEstadoJurisdiccionDto> SubTipos { get; set; }
    }
}
