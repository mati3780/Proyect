using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECT.WebAPI.DTOs
{
    public class SearchCompensacionesDto : BusquedaDto
    {
        public Int64? JurisdiccionId { get; set; }
        public String FechaDesde { get; set; }
        public String FechaHasta { get; set; }
    }
}