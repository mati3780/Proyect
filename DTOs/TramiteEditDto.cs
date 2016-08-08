using System;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteEditDto : IRectificacionDto
    {
        public bool EsRectificacionSolicitante { get; set; }
        public bool EsRectificacionPersona { get; set; }
        public bool EsRectificacionInmueble { get; set; }
        public Int64 Id { get; set; }
        public Int64 JurisdiccionId { get; set; }
        public Int64 PlazoId { get; set; }
        public String Numero { get; set; }
        public String MotivoConsulta { get; set; }
        public String DatosAdicionales { get; set; }
        public String Observacion { get; set; }
        public String ObservacionEntidad { get; set; }
        public String ObservacionSolicitante { get; set; }
        public SolicitudEntidadDto Entidad { get; set; }
        public SolicitanteDto Solicitante { get; set; }
    }
}
