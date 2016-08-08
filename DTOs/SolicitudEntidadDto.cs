namespace PROYECT.WebAPI.DTOs
{
    using System;

    public class SolicitudEntidadDto
    {
        public Int64 Id { get; set; }
        public PersonaDto Persona { get; set; }
        public InmuebleDto Inmueble { get; set; }
    }
}
