using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class TramiteEstadoExtension
    {
        public static TramiteEstadoDto Map(this TramiteEstado value)
        {
            if (value == null)
                return null;


            var model = new TramiteEstadoDto
            {
                Fecha = value.Fecha.ToShort(),
                Observacion = value.Observacion,
                TramiteEstadoId = value.SubTipo.TipoEstado.Id,
                TramiteEstadoDescripcion = value.SubTipo.TipoEstado.Descripcion,
                TramiteSubEstadoId = value.SubTipo.Id,
                TramiteSubEstadoDescripcion = value.SubTipo.Descripcion,
                TramiteEstadoIdentificador = value.SubTipo.TipoEstado.Identificador,
                TramiteSubEstadoIdentificador = value.SubTipo.Identificador,
                TramiteEstadoComentario = value.SubTipo.TipoEstado.Comentario,
                TramiteSubEstadoComentario = value.SubTipo.Comentario
            };

            return model;
        }

        public static IList<TramiteEstadoDto> Map(this IList<TramiteEstado> value)
        {
            if (value == null)
                return new List<TramiteEstadoDto>();

            return value.Select(x => x.Map()).ToList();
        }
    }
}
