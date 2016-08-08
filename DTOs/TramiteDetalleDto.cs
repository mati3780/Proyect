using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteDetalleDto
    {
        public Int64 Id { get; set; }
        public Int64 PlazoId { get; set; }
        public Int64 JurisdiccionId { get; set; }
        public String JurisdiccionDescripcion { get; set; }
        public String PlazoDescripcion { get; set; }
        public Decimal TasaProvincial { get; set; }
        public Decimal TasaNacional { get; set; }
        public TramiteEstadoDto Estado { get; set; }
    }
}
