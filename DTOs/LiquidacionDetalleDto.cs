using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECT.WebAPI.DTOs
{
    public class    LiquidacionDetalleDto
    {
        public Int64 Id { get; set; }
        public string FechaTramite { get; set; }
        public string NumeroTramite { get; set; }
        public string Movimiento { get; set; }
        public decimal TasaNacional { get; set; }
        public decimal TasaProvincial { get; set; }
        public decimal ImporteProyect { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
    }
}