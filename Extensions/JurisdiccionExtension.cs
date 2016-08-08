using System;
using System.Linq;
using System.Security.Claims;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class JurisdiccionExtension
    {
        public static JurisdiccionDto Map(this Jurisdiccion value)
        {
            if (value == null)
                return null;

            var model = new JurisdiccionDto
            {
                Id = value.Id,
                Sigla = value.Sigla,
                CodigoPostal = value.CodigoPostal,
                Cuit = value.Cuit,
                Autoridad = value.Autoridad,
                Descripcion = value.Descripcion,
                Email1 = value.Email1,
                Telefono = value.Telefono,
                Direccion = value.Direccion,
                Email2 = value.Email2,
                LocalidadId = value.LocalidadId,
                LocalidadDescripcion = value.Localidad?.Descripcion ?? String.Empty,
                PlazoRetiroDocumentacion = value.PlazoRetiroDocumentacion,
                PlazoObservacion = value.PlazoObservacion,
                RecibirMail = value.RecibirMail,
                Url = value.Url,
                ProvinciaId = value.Provincia.Id,
                ProvinciaDescripcion = value.Provincia.Descripcion,
                TipoEstadoAdhesionDescripcion = value.TipoEstadoAdhesion.Descripcion,
                CuentasBancarias = value.CuentasBancarias.Select(x => x.Map()).ToList()
            };

            if (ClaimsPrincipal.Current.IsAdmin())
            {
                model.Orden = value.Orden;
                model.TipoEstadoAdhesionId = value.TipoEstadoAdhesionId;
                model.GrupoAd = value.GrupoAd;
                model.Observacion = value.Observacion;
            }
            
            return model;
        }
        public static SeleccionableDto MapSimple(this Jurisdiccion value)
        {
            if (value == null)
                return null;

            return new SeleccionableDto
            {
                Id = value.Id,
                Descripcion = $"{value.Descripcion} - {value.Sigla} - {value.Provincia.Descripcion}"
            };
        }
        public static Jurisdiccion Map(this JurisdiccionDto value, Jurisdiccion item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new Jurisdiccion();

            if (ClaimsPrincipal.Current.IsAdmin())
            {
                item.Orden = value.Orden;
                item.GrupoAd = value.GrupoAd.TrimSafe();
                item.Descripcion = value.Descripcion.TrimSafe();
                item.ProvinciaId = value.ProvinciaId;

                if (item.TipoEstadoAdhesionId != value.TipoEstadoAdhesionId)
                    item.AddEstadoAdhesion(value.TipoEstadoAdhesionId);
            }
            else
            {
                item.Sigla = value.Sigla.TrimSafe();
                item.CodigoPostal = value.CodigoPostal.TrimSafe();
                item.Cuit = value.Cuit;
                item.Autoridad = value.Autoridad.TrimSafe();
                item.Email1 = value.Email1.TrimSafe();
                item.Telefono = value.Telefono.TrimSafe();
                item.Direccion = value.Direccion.TrimSafe();
                item.Email2 = value.Email2.TrimSafe();
                item.LocalidadId = value.LocalidadId;
                item.ProvinciaId = value.ProvinciaId;
                item.PlazoRetiroDocumentacion = value.PlazoRetiroDocumentacion;
                item.PlazoObservacion = value.PlazoObservacion;
                item.RecibirMail = value.RecibirMail;
                item.Url = value.Url.TrimSafe();
                item.Observacion = value.Observacion.TrimSafe();
            }

            return item;
        }
        public static TipoEstadoJurisdiccionDto Map(this TipoEstadoTramite value)
        {
            var model = new TipoEstadoJurisdiccionDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden,
                SubTipos = value.SubTipos.Select(x => x.Map()).ToList()
            };

            return model;
        }
        public static SubTipoEstadoJurisdiccionDto Map(this SubTipoEstadoTramite value)
        {
            var model = new SubTipoEstadoJurisdiccionDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden
            };

            return model;
        }
        public static PlazoDto Map(this Plazo value)
        {
            var model = new PlazoDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
    }
}
