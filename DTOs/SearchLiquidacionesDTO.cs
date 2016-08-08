using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECT.WebAPI.DTOs
{
    public class SearchLiquidacionesDTO : BusquedaDto
    {
        public Int64? JurisdiccionId { get; set; }
        public bool EsEnteOrigenDestino { get; set; }
        public String FechaDesde { get; set; }
        public String FechaHasta { get; set; }
        public Int16? Movimiento { get; set; }
        public Int16? Pago { get; set; }
        public bool NoCompensados { get; set; }
        
    }
}