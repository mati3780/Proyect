using System;
using PROYECT.Dominio.Enums;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteEstadoDto
    {
        public String Fecha { get; set; }
        public TipoEstadoTramiteIdentificador TramiteEstadoIdentificador { get; set; }
        public Int64 TramiteEstadoId { get; set; }
        public String TramiteEstadoDescripcion { get; set; }
        public String TramiteEstadoComentario { get; set; }
        public SubTipoEstadoTramiteIdentificador TramiteSubEstadoIdentificador { get; set; }
        public Int64 TramiteSubEstadoId { get; set; }
        public String TramiteSubEstadoDescripcion { get; set; }
        public String TramiteSubEstadoComentario { get; set; }
        public String Observacion { get; set; }
    }
}
