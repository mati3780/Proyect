using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
    public static class SolicitudFilterRuleExtension
    {
        public static IQueryable<Solicitud> Filter(this IQueryable<Solicitud> query, SolicitudBusquedaDto dto)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
            var estados = new List<TipoEstadoSolicitudIdentificador> { TipoEstadoSolicitudIdentificador.PresentacionPendiente };
            query = query.Where(x => x.JurisdiccionRequirente.Id == jurisdiccionId && estados.Contains(x.TipoEstado.Identificador));

            if (dto.NumeroDocumento > 0)
                query = query.Where(x => x.Solicitante.NumeroDocumento == dto.NumeroDocumento);

            if (dto.TipoDocumento > 0)
                query = query.Where(x => x.Solicitante.TipoDocumento.Id == dto.TipoDocumento);

            if (!String.IsNullOrEmpty(dto.NumeroTramite))
                query = query.Where(x => x.Numero == dto.NumeroTramite.Trim());

            return query;
        }
    }
}
