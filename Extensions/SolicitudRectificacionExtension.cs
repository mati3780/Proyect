using System;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitudRectificacionExtension
    {
        public static SolicitudRectificacion MapRectificacion(this Solicitud value, IRectificacionDto dto)
        {
            var item = new SolicitudRectificacion
            {
                SolicitudId = value.Id,
                Numero = value.Numero,
                EstadoDescripcion = value.TipoEstado.Descripcion,
                EstadoId = value.TipoEstado.Id,
                ServicioId = value.Servicio.Id,
                ServicioDescripcion = value.Servicio.Descripcion,
                Fecha = DateTime.Now
            };

            if (dto.EsRectificacionSolicitante)
                item.Solicitante = value.Solicitante.MapRectificacion(dto);

            if (dto.EsRectificacionInmueble || dto.EsRectificacionPersona)
                item.Entidad = value.Entidad.MapRectificacion(dto);

            return item;
        }
        public static SolicitudRectificacionDto Map(this SolicitudRectificacion value)
        {
            var model = new SolicitudRectificacionDto
            {
                TramiteId = value.SolicitudId,
                Numero = value.Numero,
                Fecha = value.Fecha,
                Solicitante = value.Solicitante.Map(),
                Entidad = value.Entidad.Map(),
                EstadoId = value.EstadoId,
                EstadoDescripcion = value.EstadoDescripcion,
                ServicioId = value.ServicioId,
                ServicioDescripcion = value.ServicioDescripcion
            };

            return model;
        }
    }
}
