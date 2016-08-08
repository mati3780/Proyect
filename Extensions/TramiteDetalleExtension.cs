using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class TramiteDetalleExtension
    {

        public static IList<TramiteDetalleDto> MapDetalle(this IList<Tramite> value)
        {
            if (!value.Any())
                return new List<TramiteDetalleDto>();

            return value.Select(x => x.MapDetalle()).ToList();
        }

        public static IList<TramiteConsultaDto> MapConsulta(this IList<Tramite> value)
        {
            if (!value.Any())
                return new List<TramiteConsultaDto>();

            return value.Select(x => x.MapConsulta()).ToList();
        }

        public static TramiteDetalleDto MapDetalle(this Tramite value)
        {
            if (value == null)
                return null;

            var model = new TramiteDetalleDto
            {
                Id = value.Id,
                PlazoId = value.JurisdiccionPlazoId,
                JurisdiccionId = value.Jurisdiccion.Id,
                JurisdiccionDescripcion = $"{value.Jurisdiccion.Descripcion} - {value.Jurisdiccion.Sigla} - {value.Jurisdiccion.Provincia.Descripcion}",
                PlazoDescripcion = value.JurisdiccionPlazo.Plazo.Descripcion,
                TasaProvincial = value.GetCostoProvincial(),
                TasaNacional = value.Solicitud.GetCostoNacional(),
                Estado = value.Estados.OrderBy(x => x.Id).Last().Map()
            };

            return model;
        }

        public static TramiteConsultaDto MapConsulta(this Tramite value)
        {
            if (value == null)
                return null;

            var model = new TramiteConsultaDto
            {
                Id = value.Id,
                ProvinciaDescripcion = value.Jurisdiccion.Provincia.Descripcion,
                JurisdiccionDescripcion = $"{value.Jurisdiccion.Descripcion} - {value.Jurisdiccion.Sigla}",
                PlazoDescripcion = value.JurisdiccionPlazo.Plazo.Descripcion,
                EstadoComentario = value.SubTipoEstado.Comentario
            };

            return model;
        }

        public static IList<Tramite> MapDetalle(this IList<TramiteDetalleDto> value)
        {
            if (!value.Any())
                return new List<Tramite>();

            return value.Select(x => x.MapDetalle()).ToList();
        }

        public static Tramite MapDetalle(this TramiteDetalleDto value)
        {
            if (value == null)
                return null;

            return new Tramite
            {
                JurisdiccionId = value.JurisdiccionId,
                JurisdiccionPlazoId = value.PlazoId
            };
        }

    }
}
