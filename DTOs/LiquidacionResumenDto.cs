using System;

namespace PROYECT.WebAPI.DTOs
{
    public class LiquidacionResumenDto
    {
        public Int64 Id { get; set; }
        public string Movimiento { get; set; }
        public int MovimientoEnumValue { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Importe { get; set; }
        public String Fecha { get; set; }
    }
}