using System.Linq;
using LinqKit;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
    public static class JurisdiccionFilterRuleExtension
    {
        public static IQueryable<Jurisdiccion> Filter(this IQueryable<Jurisdiccion> query, JurisdiccionBusquedaDto criterio)
        {
            var expr = PredicateBuilder.True<Jurisdiccion>();

            if (!string.IsNullOrWhiteSpace(criterio.Jurisdiccion))
                expr = expr.And(x => x.Descripcion.ToLower().Contains(criterio.Jurisdiccion.ToLower()));

            if (criterio.ProvinciaId.HasValue && criterio.ProvinciaId > 0)
                expr = expr.And(x => x.ProvinciaId == criterio.ProvinciaId.Value);

            if (criterio.TipoEstadoAdhesionId.HasValue && criterio.TipoEstadoAdhesionId > 0)
                expr = expr.And(x => x.TipoEstadoAdhesionId == criterio.TipoEstadoAdhesionId.Value);

            query = query.AsExpandable().Where(expr);

            return query;
        }
    }
}