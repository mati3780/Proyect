using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudRectificacionDto
    {
        public Int64 TramiteId { get; set; }
        public String Numero { get; set; }
        public String MotivoConsulta { get; set; }
        public Int64 EstadoId { get; set; }
        public String EstadoDescripcion { get; set; }
        public Int64 ServicioId { get; set; }
        public String ServicioDescripcion { get; set; }
        public SolicitanteRectificacionDto Solicitante { get; set; }
        public SolicitudEntidadRectificacionDto Entidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}