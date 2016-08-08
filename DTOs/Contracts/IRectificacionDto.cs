using System;

namespace PROYECT.WebAPI.DTOs.Contracts
{
    public interface IRectificacionDto
    {
        bool EsRectificacionSolicitante { get; set; }
        bool EsRectificacionPersona { get; set; }
        bool EsRectificacionInmueble { get; set; }
        Int64 Id { get; set; }
        String Numero { get; set; }
        String Observacion { get; set; }
        String ObservacionEntidad { get; set; }
        String ObservacionSolicitante { get; set; }
        SolicitudEntidadDto Entidad { get; set; }
        SolicitanteDto Solicitante { get; set; }
    }
}