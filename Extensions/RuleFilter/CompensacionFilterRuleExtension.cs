using System;
using System.Linq;
using System.Security.Claims;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
    public static class CompensacionFilterRuleExtension
    {
        public static PagingExtensions.Page<CompensacionResumenDto> GetJurisdiccion(this IQueryable<Compensacion> query, SearchCompensacionesDto dto)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
            query = query.Where(x => x.Jurisdiccion.Id == jurisdiccionId);
            query = query.Filter(dto);

            return query.ToPage(dto.Start, dto.Length, r => r.MapResumen());
        }

        public static PagingExtensions.Page<CompensacionResumenDto> GetEnte(this IQueryable<Compensacion> query, SearchCompensacionesDto dto)
        {
            query = query.Filter(dto);
            return query.ToPage(dto.Start, dto.Length, r => r.MapResumen());
        }
        
        private static IQueryable<Compensacion> Filter(this IQueryable<Compensacion> query, SearchCompensacionesDto dto)
        {
            if (!String.IsNullOrEmpty(dto.FechaDesde))
            {
                var fechaDesde = dto.FechaDesde.ToDate();
                query = query.Where(x => x.Fecha >= fechaDesde);
            }

            if (!String.IsNullOrEmpty(dto.FechaHasta))
            {
                var fechaHasta = dto.FechaHasta.ToDate();
                query = query.Where(x => x.Fecha <= fechaHasta);
            }

            if (dto.JurisdiccionId != null && dto.JurisdiccionId.Value > 0)
                query = query.Where(x => x.Jurisdiccion.Id == dto.JurisdiccionId.Value);

            return query.OrderBy(x => x.Fecha);
        }
    }
}