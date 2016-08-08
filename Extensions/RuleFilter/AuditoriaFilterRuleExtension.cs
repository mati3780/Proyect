using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using Helper.Dominio.Model.Entities;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
	public static class AuditoriaFilterRuleExtension
	{
		public static IQueryable<AuditoriaOperacion> Filter(this IQueryable<AuditoriaOperacion> query, AuditoriaBusquedaDto criterio)
		{
			var expr = PredicateBuilder.True<AuditoriaOperacion>();

			DateTime fechaInicio, fechaFin;
			if (DateTime.TryParseExact(criterio.FechaOperacionInicial, "dd/MM/yyyy", new CultureInfo("es-AR"), DateTimeStyles.AssumeLocal, out fechaInicio))
				expr = expr.And(ao => ao.FechaOperacion >= fechaInicio);

			if (DateTime.TryParseExact(criterio.FechaOperacionFinal, "dd/MM/yyyy", new CultureInfo("es-AR"), DateTimeStyles.AssumeLocal, out fechaFin))
				expr = expr.And(ao => ao.FechaOperacion <= fechaFin);

			if (!string.IsNullOrWhiteSpace(criterio.Usuario))
				expr = expr.And(ao => ao.Usuario.Contains(criterio.Usuario));

			if (!string.IsNullOrWhiteSpace(criterio.Ip))
				expr = expr.And(ao => ao.Ip == criterio.Ip);

			if (!string.IsNullOrWhiteSpace(criterio.Url))
				expr = expr.And(ao => ao.Url.Contains(criterio.Url));

			if (!string.IsNullOrWhiteSpace(criterio.Navegador))
				expr = expr.And(ao => ao.Navegador.Contains(criterio.Navegador));

			if (!string.IsNullOrWhiteSpace(criterio.EntidadNombre))
				expr = expr.And(ao => ao.AuditoriaOperacionDetalles.Any(aod => aod.EntidadNombre.Contains(criterio.EntidadNombre)));

			if (!string.IsNullOrWhiteSpace(criterio.EntidadKey))
				expr = expr.And(ao => ao.AuditoriaOperacionDetalles.Any(aod => aod.EntidadKey.Contains(criterio.EntidadKey)));

			if (!string.IsNullOrWhiteSpace(criterio.Accion))
				expr = expr.And(ao => ao.AuditoriaOperacionDetalles.Any(aod => aod.Accion == criterio.Accion));

			if (!string.IsNullOrWhiteSpace(criterio.JsonOperacion))
				expr = expr.And(ao => ao.AuditoriaOperacionDetalles.Any(aod => aod.JsonOperacion.Contains(criterio.JsonOperacion)));

			query = query.AsExpandable().Where(expr);

			return query;
		}
	}
}
