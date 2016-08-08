using System;
using PROYECT.Dominio.Enums;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteResumenDto
    {
        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String FechaTramite { get; set; }
        public String FechaEntrega { get; set; }
        public String EstadoSolicitudDescripcion { get; set; }
        public String EstadoDescripcion { get; set; }
        public Int64 SubTipoEstadoId { get; set; }
        public String SubEstadoDescripcion { get; set; }
        public String JurisdiccionRequiriente { get; set; }
        public String JurisdiccionInformante { get; set; }
        public String ServicioDescripcion { get; set; }
        public String PlazoDescripcion { get; set; }
        public Int32 DiasCursados { get; set; }
        public TramiteEstadoVencimiento Estado { get; set; }
        public Decimal TasaProvincial { get; set; }
        public Decimal TasaNacional { get; set; }
    }
}
