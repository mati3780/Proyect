using System;
using System.Security.Claims;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class FeriadoExtension
    {
        public static FeriadoDto Map(this Feriado value)
        {
            if (value == null)
                return null;

            var model = new FeriadoDto
                        {
                            Id = value.Id,
                            Fecha = value.Fecha.ToShort(),
                            Descripcion = string.Format($"{value.Descripcion}{(ClaimsPrincipal.Current.IsAdmin() && value.JurisdiccionId.HasValue ? $" - {value.Jurisdiccion.Descripcion}" : string.Empty)}")
                        };

            return model;
        }

        public static Feriado Map(this FeriadoDto value, Feriado item)
        {
            if (item == null)
                item = new Feriado();
            
            item.Descripcion = value.Descripcion;
            item.Fecha = value.Fecha.ToDate();

            if (!ClaimsPrincipal.Current.IsAdmin())
                item.JurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();

            return item;
        }
        
    }
}
