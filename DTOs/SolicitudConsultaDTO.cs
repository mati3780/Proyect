using System;
using System.Collections.Generic;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudConsultaDTO
    {
        public SolicitudConsultaDTO()
        {
            Tramites = new List<TramiteConsultaDto>();
        }
        
        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String EstadoComentario { get; set; }
        public ServicioDto Servicio { get; set; }
        public SolicitanteDto Solicitante { get; set; }
        public PagoDto Pago { get; set; }
        public JurisdiccionDto JurisdiccionRequirente { get; set; }
        public JurisdiccionDto ReparticionSolicitante { get; set; }
        public SolicitudEntidadDto Entidad { get; set; }
        public IList<TramiteConsultaDto> Tramites { get; set; }
    }
}
