using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudEntidadRectificacionDto
    {
        public Int64 TramiteEntidadId { get; set; }
        public PersonaRectificacionDto Persona { get; set; }
        public InmuebleRectificacionDto Inmueble { get; set; }
    }
}
