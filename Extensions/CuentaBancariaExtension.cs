using System;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using System.Security.Claims;
using PROYECT.Dominio.Extensions.Common;

namespace PROYECT.WebAPI.Extensions
{
    public static class CuentaBancariaExtension
    {
        public static CuentaBancariaDto Map(this CuentaBancaria value)
        {
            if (value == null)
                return null;

            var model = new CuentaBancariaDto
            {
                Id = value.Id,
                Entidad = value.Entidad,
                Sucursal = value.Sucursal,
                TipoId = value.TipoId,
                Tipo = value.Tipo.Descripcion,
                Numero = value.Numero,
                CBU = value.CBU,
                Principal = value.Principal,
                FechaVigencia = value.FechaVigencia?.ToShort(),
                Jurisdiccion = value.Jurisdiccion.Descripcion
            };

            return model;
        }
        public static CuentaBancaria Map(this CuentaBancariaDto value, CuentaBancaria item)
        {
            if (item == null)
                item = new CuentaBancaria();
            
            item.Entidad = value.Entidad;
            item.Sucursal = value.Sucursal;
            item.TipoId = value.TipoId;
            item.Numero = value.Numero;
            item.CBU = value.CBU;
            item.Principal = value.Principal;
            item.JurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();

            return item;
        }
        public static CuentaBancaria MapSimple(this CuentaBancariaDto value, CuentaBancaria item)
        {
            
            item.FechaVigencia = value.FechaVigencia.ToDate();
            return item;
        }
        
    }
}
