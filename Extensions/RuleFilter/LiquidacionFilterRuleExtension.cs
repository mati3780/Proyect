using System;
using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
    public static class LiquidacionFilterRuleExtension
    {
        public static IQueryable<Liquidacion> Filter(this IQueryable<Liquidacion> query, SearchLiquidacionesDTO dto)
        {
            DateTime fechaDesde;
            if (!string.IsNullOrEmpty(dto.FechaDesde) && DateTime.TryParse(dto.FechaDesde, out fechaDesde))
            {
                query = query.Where(x => x.FechaCorteHasta >= fechaDesde);
            }

            DateTime fechaHasta;
            if (!string.IsNullOrEmpty(dto.FechaHasta) && DateTime.TryParse(dto.FechaHasta, out fechaHasta))
            {
                query = query.Where(x => x.FechaCorteHasta <= fechaHasta);
            }

            if (dto.Movimiento.HasValue)
            {
                if (dto.Movimiento == 2)//Depósito
                {
                    query = query.Where(x => x.OrigenId == dto.JurisdiccionId);
                }
                else//Acreditación
                {
                    query = query.Where(x => x.OrigenId != dto.JurisdiccionId || (x.OrigenId == dto.JurisdiccionId && x.DestinoId == dto.JurisdiccionId && x.Deposito != null));
                }
            }

            if (dto.Pago.HasValue)
            {
                query = dto.Pago == 1 ? query.Where(x => x.Deposito != null) : query.Where(x => x.Deposito == null);
            }

            if (dto.NoCompensados)
            {
                query = query.Where(x => x.Compensacion == null);
            }

            return query;
        }
    }
}