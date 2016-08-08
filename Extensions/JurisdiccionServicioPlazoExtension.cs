using System.Security.Claims;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class JurisdiccionServicioPlazoExtension
    {
        public static JurisdiccionServicioPlazoDto Map(this JurisdiccionServicioPlazo value)
        {
            if (value == null)
                return null;

            var model = new JurisdiccionServicioPlazoDto
            {
                Id = value.Id,
                PlazoDias = value.PlazoDias,
                Costo = value.Costo,
                VigenciaDesde = value.VigenciaDesde.ToShort(),
                VigenciaHasta = value.VigenciaHasta.HasValue ? value.VigenciaHasta.Value.ToShort() : null,
                ServicioId = value.ServicioId,
                ServicioDescripcion = value.Servicio.Descripcion,
                PlazoId = value.PlazoId,
                PlazoDescripcion = value.Plazo.Descripcion
            };

            return model;
        }

        public static JurisdiccionServicioPlazoDto Map(this JurisdiccionServicioPlazo value, Jurisdiccion reparticion)
        {
            if (value == null)
                return null;

            var model = new JurisdiccionServicioPlazoDto
            {
                Id = value.Id,
                PlazoDias = value.PlazoDias,
                Costo = value.GetCostoProvincial(reparticion),
                VigenciaDesde = value.VigenciaDesde.ToShort(),
                VigenciaHasta = value.VigenciaHasta?.ToShort(),
                ServicioId = value.ServicioId,
                ServicioDescripcion = value.Servicio.Descripcion,
                PlazoId = value.PlazoId,
                PlazoDescripcion = value.Plazo.Descripcion
            };

            return model;
        }

        public static JurisdiccionServicioPlazo Map(this JurisdiccionServicioPlazoDto value, JurisdiccionServicioPlazo item)
        {
            if (item == null)
                item = new JurisdiccionServicioPlazo();

            item.JurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();

            item.PlazoDias = value.PlazoDias;
            item.Costo = value.Costo;
            item.VigenciaDesde = value.VigenciaDesde.ToDate();
            item.PlazoId = value.PlazoId;
            item.ServicioId = value.ServicioId;

            return item;
        }
    }
}