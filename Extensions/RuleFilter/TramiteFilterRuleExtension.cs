using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions.RuleFilter
{
    public static class TramiteFilterRuleExtension
    {
        public static PagingExtensions.Page<TramiteResumenDto> GetRequiriente(this IQueryable<Tramite> query, TramiteBusquedaDto dto,
                                                                                    IList<Feriado> feriadosNacionales)
        {
            query = query.FilterRequiriente(dto);
            return query.ToPage(dto.Start, dto.Length, r => r.MapResumen(feriadosNacionales));
        }

        public static PagingExtensions.Page<TramiteResumenDto> GetInformante(this IQueryable<Tramite> query, TramiteBusquedaDto dto,
                                                                                    IList<Feriado> feriadosNacionales)
        {
            query = query.FilterInformante(dto);
            return query.ToPage(dto.Start, dto.Length, r => r.MapResumen(feriadosNacionales));
        }

        private static IQueryable<Tramite> FilterRequiriente(this IQueryable<Tramite> query, TramiteBusquedaDto dto)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();

            //Todos los TipoEstadoId = 2 - En Trámite
            var estados = new List<SubTipoEstadoTramiteIdentificador>
            {
                SubTipoEstadoTramiteIdentificador.EnTramite,
                SubTipoEstadoTramiteIdentificador.DocumentacionEnTramite,
                SubTipoEstadoTramiteIdentificador.Observado,
                SubTipoEstadoTramiteIdentificador.Rectificado,
                SubTipoEstadoTramiteIdentificador.SeRegistroPdf,
                SubTipoEstadoTramiteIdentificador.SeVerifico,
                SubTipoEstadoTramiteIdentificador.ElPdfNoCorrespondeInforme,
                SubTipoEstadoTramiteIdentificador.ElPdfNoSeVisualiza,
                SubTipoEstadoTramiteIdentificador.Entregado
            };

            query = query.Where(x => x.Solicitud.JurisdiccionRequirente.Id == jurisdiccionId && estados.Contains(x.SubTipoEstado.Identificador));
            query = query.Filter(dto);

            return query;
        }

        private static IQueryable<Tramite> FilterInformante(this IQueryable<Tramite> query, TramiteBusquedaDto dto)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();

            //Todos los TipoEstadoId = 2 - En Trámite
            var estados = new List<SubTipoEstadoTramiteIdentificador>
            {
                SubTipoEstadoTramiteIdentificador.EnTramite,
                SubTipoEstadoTramiteIdentificador.DocumentacionEnTramite,
                SubTipoEstadoTramiteIdentificador.Observado,
                SubTipoEstadoTramiteIdentificador.Rectificado,
                SubTipoEstadoTramiteIdentificador.SeRegistroPdf,
                SubTipoEstadoTramiteIdentificador.SeVerifico,
                SubTipoEstadoTramiteIdentificador.ElPdfNoCorrespondeInforme,
                SubTipoEstadoTramiteIdentificador.ElPdfNoSeVisualiza,
                SubTipoEstadoTramiteIdentificador.Entregado
            };

            query = query.Where(x => x.Jurisdiccion.Id == jurisdiccionId && estados.Contains(x.SubTipoEstado.Identificador));

            query = query.Filter(dto);

            return query;
        }

        private static IQueryable<Tramite> Filter(this IQueryable<Tramite> query, TramiteBusquedaDto dto)
        {
            if (dto.NumeroDocumento > 0)
                query = query.Where(x => x.Solicitud.Solicitante.NumeroDocumento == dto.NumeroDocumento);

            if (dto.TipoDocumento > 0)
                query = query.Where(x => x.Solicitud.Solicitante.TipoDocumento.Id == dto.TipoDocumento);

            if (dto.EstadoSolicitud.HasValue)
                query = query.Where(x => x.Solicitud.TipoEstado.Id == dto.EstadoSolicitud.Value);

            if (dto.EstadoTramite.HasValue)
                query = dto.SubEstadoTramite.HasValue
                                         ? query.Where(t => t.SubTipoEstado.TipoEstado.Id == dto.EstadoTramite.Value &&
                                                            t.SubTipoEstadoId == dto.SubEstadoTramite.Value)
                                         : query.Where(t => t.SubTipoEstado.TipoEstado.Id == dto.EstadoTramite.Value);

            if (!String.IsNullOrEmpty(dto.NumeroTramite))
                query = query.Where(x => x.Solicitud.Numero == dto.NumeroTramite.Trim());

            return query.OrderBy(x => x.Solicitud.FechaRegistracion);
        }

        public static IQueryable<Tramite> FilterAdminEnte(this IQueryable<Tramite> query, TramiteBusquedaAdminEnteDto dto)
        {
            if (dto.Jurisdiccion.HasValue)
                query = (dto.TipoJurisdiccion.HasValue && dto.TipoJurisdiccion.Value)
                            ? query.Where(t => t.Solicitud.JurisdiccionRequirenteId == dto.Jurisdiccion.Value)
                            : query.Where(t => t.JurisdiccionId == dto.Jurisdiccion.Value);

            if (dto.TipoDocumento.HasValue)
                query = query.Where(t => t.Solicitud.Solicitante.TipoDocumentoId == dto.TipoDocumento.Value);

            if (dto.NumeroDocumento.HasValue)
                query = query.Where(t => t.Solicitud.Solicitante.NumeroDocumento == dto.NumeroDocumento.Value);

            if (dto.EstadoSolicitud.HasValue)
                query = query.Where(t => t.Solicitud.TipoEstadoId == dto.EstadoSolicitud.Value);

            if (dto.Estado.HasValue)
                query = dto.SubEstado.HasValue
                         ? query.Where(t => t.SubTipoEstado.TipoEstado.Id == dto.Estado.Value &&
                                            t.SubTipoEstadoId == dto.SubEstado.Value)
                         : query.Where(t => t.SubTipoEstado.TipoEstado.Id == dto.Estado.Value);

            if (!string.IsNullOrEmpty(dto.CodigoBarra))
                query = query.Where(t => t.Solicitud.Numero == dto.CodigoBarra);

            if (dto.TipoPersona.HasValue)
            {

                query = dto.TipoPersona.Value
                                  ? query.Where(t => t.Solicitud.Entidad.Persona != null && t.Solicitud.Entidad.Persona is PersonaFisica)
                                  : query.Where(t => t.Solicitud.Entidad.Persona != null && t.Solicitud.Entidad.Persona is PersonaJuridica);
                if (!string.IsNullOrEmpty(dto.ApellidoDenominacion))
                    query = dto.TipoPersona.Value
                                        ? query.Where(t => (t.Solicitud.Entidad.Persona as PersonaFisica).Apellido == dto.ApellidoDenominacion)
                                        : query.Where(t => (t.Solicitud.Entidad.Persona as PersonaJuridica).RazonSocial == dto.ApellidoDenominacion);
            }

            return query.OrderBy(x => x.Solicitud.FechaRegistracion);

        }

    }
}
