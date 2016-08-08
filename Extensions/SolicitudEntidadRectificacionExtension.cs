using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitudEntidadRectificacionExtension
    {
        public static SolicitudEntidadRectificacionDto Map(this SolicitudEntidadRectificacion value)
        {
            var model = new SolicitudEntidadRectificacionDto
            {
                TramiteEntidadId = value.TramiteEntidadId,
                Persona = value.Persona.Map(),
                Inmueble = value.Inmueble.Map()
            };

            return model;
        }
    }
}
