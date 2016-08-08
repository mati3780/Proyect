using System;

namespace PROYECT.WebAPI.DTOs
{
    public class JurisdiccionServicioPlazoDto
    {
        public Int64 Id { get; set; }
        public Int16 PlazoDias { get; set; }
        public Decimal Costo { get; set; }
        public String VigenciaDesde { get; set; }
        public String VigenciaHasta { get; set; }
        public DateTime EntidadVigenciaDesde { get; set; }
        public DateTime? EntidadVigenciaHasta { get; set; }
        public Int64 ServicioId { get; set; }
        public Int64 PlazoId { get; set; }
        public String ServicioDescripcion { get; set; }
        public String PlazoDescripcion { get; set; }
    }
}