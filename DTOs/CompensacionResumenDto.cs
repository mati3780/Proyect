using System;

namespace PROYECT.WebAPI.DTOs
{
    public class CompensacionResumenDto
    {
        public Int64 Id { get; set; }
        public String Fecha { get; set; }
        public Decimal Importe { get; set; }
        public String Jurisdiccion { get; set; }
        public String FechaDeposito { get; set; }
    }
}
