using System;
using System.Collections.Generic;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteDto
    {
        public TramiteDto()
        {
            Tramites = new List<TramiteDetalleDto>();
            Estados = new List<TramiteEstadoDto>();
        }

        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String MotivoConsulta { get; set; }
        public String DatosAdicionales { get; set; }
        public SeleccionableDto JurisdiccionRequirente { get; set; }
        public SeleccionableDto ReparticionSolicitante { get; set; }
        public ServicioDto Servicio { get; set; }
        public SolicitanteDto Solicitante { get; set; }
        public PagoDto Pago { get; set; }
        public SolicitudEntidadDto Entidad { get; set; }
        public TramiteEstadoDto EstadoActual { get; set; }
        public IList<TramiteDetalleDto> Tramites { get; set; }
        public IList<TramiteEstadoDto> Estados { get; set; }
        public Int64 SolicitudId { get; set; }
    }
}
