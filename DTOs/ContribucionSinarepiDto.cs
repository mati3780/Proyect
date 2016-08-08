using System;

namespace PROYECT.WebAPI.DTOs
{
    public class ContribucionProyectDto
    {
        public Int64 Id { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public decimal Valor { get; set; }
    }
}