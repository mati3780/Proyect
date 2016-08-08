using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    static class TomoFolioExtension
    {
        public static IList<TomoFolioDto> Map(this IList<TomoFolio> value)
        {
            if (!value.Any())
                return new List<TomoFolioDto>();

            return value.Select(x => x.Map()).ToList();
        }
        public static IList<TomoFolio> Map(this IList<TomoFolioDto> value, IList<TomoFolio> items)
        {
            var results = new List<TomoFolio>();

            if (!value.Any())
                return results;

            foreach (var tomoFolio in value)
            {
                var entidad = items.SingleOrDefault(x => x.Id == tomoFolio.Id);

                if (entidad != null && tomoFolio.Borrado)
                    items.Remove(entidad);
                else
                {
                    var result = tomoFolio.Map(entidad);
                    if (result.Id == 0)
                        results.Add(result);
                }
            }

            foreach (var r in results)
                items.Add(r);

            return items;
        }
        public static IList<TomoFolioRectificacion> MapRectificacion(this IList<TomoFolio> value, IRectificacionDto dto)
        {
            if (!value.Any())
                return new List<TomoFolioRectificacion>();

            var tomosFolio = new List<TomoFolioRectificacion>();
            foreach (var item in dto.Entidad.Inmueble.TomosFolios.Where(x => x.Borrado).ToList())
            {
                var val = value.SingleOrDefault(x => x.Id == item.Id);
                if (val != null)
                    tomosFolio.Add(val.MapRectificacion(item));
            }

            return tomosFolio;
        }
        public static TomoFolioDto Map(this TomoFolio value)
        {
            if (value == null)
                return null;

            var model = new TomoFolioDto
            {
                Id = value.Id,
                Tomo = value.Tomo,
                Folio = value.Folio
            };

            return model;
        }
        public static TomoFolio Map(this TomoFolioDto value, TomoFolio item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new TomoFolio();

            item.Tomo = value.Tomo;
            item.Folio = value.Folio;

            return item;
        }
        private static TomoFolioRectificacion MapRectificacion(this TomoFolio value, TomoFolioDto dto)
        {
            var item = new TomoFolioRectificacion
            {
                Tomo = value.Tomo,
                Folio = value.Folio,
                TomoFolioId = 1,		//TODO: ESTO ESTA BIEN ASI HARDCODEADO!?!?
                Observacion = dto.Observacion
            };

            return item;
        }
    }
}