using System;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitudEntidadExtension
    {
        public static SolicitudEntidadDto Map(this SolicitudEntidad value)
        {
            if (value == null)
                return null;

            var model = new SolicitudEntidadDto
            {
                Id = value.Id,
                Persona = value.Persona.Map(),
                Inmueble = value.Inmueble.Map()
            };

            return model;
        }
        public static SolicitudEntidad Map(this SolicitudEntidadDto value, SolicitudEntidad item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new SolicitudEntidad();

            if (value.Persona != null)
                item.Persona = value.Persona.Map(item.Persona);

            if (value.Inmueble != null)
                item.Inmueble = value.Inmueble.Map(item.Inmueble);

            return item;
        }
        public static SolicitudEntidadRectificacion MapRectificacion(this SolicitudEntidad value, IRectificacionDto dto)
        {
            var item = new SolicitudEntidadRectificacion
                       {
                           TramiteEntidadId = value.Id,
                           Fecha = DateTime.Now,
                           Observacion = dto.ObservacionEntidad
                       };

            if (dto.EsRectificacionPersona && value.Persona != null)
                item.Persona = value.Persona.MapRectificacion();

            if (dto.EsRectificacionInmueble && value.Inmueble != null)
                item.Inmueble = value.Inmueble.MapRectificacion(dto);

            return item;
        }
    }
}
