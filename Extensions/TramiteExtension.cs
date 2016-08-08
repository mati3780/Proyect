using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Helpers;

namespace PROYECT.WebAPI.Extensions
{
    public static class TramiteExtension
    {

        public static TramiteDto Map(this Tramite value)
        {
            if (value == null)
                return null;

            var model = new TramiteDto
            {
                Id = value.Id,
                Numero = value.Solicitud.Numero,
                DatosAdicionales = value.Solicitud.DatosAdicionales,
                MotivoConsulta = value.Solicitud.MotivoConsulta,
                Tramites = value.Solicitud.Tramites.MapDetalle(),
                Solicitante = value.Solicitud.Solicitante.Map(),
                Entidad = value.Solicitud.Entidad.Map(),
                JurisdiccionRequirente = value.Solicitud.JurisdiccionRequirente.MapSimple(),
                ReparticionSolicitante = value.Solicitud.ReparticionSolicitante.MapSimple(),
                Servicio = value.Solicitud.Servicio.MapSimple(),
                Pago = value.Solicitud.Pago.Map(),
                Estados = value.Estados.OrderByDescending(x => x.Id).ToList().Map(),
                EstadoActual = value.Estados.OrderByDescending(x => x.Id).First().Map(),
                SolicitudId = value.Solicitud.Id
            };

            if (model.EstadoActual != null)
            {
                model.Estados.RemoveAt(0);
            }

            return model;
        }
        public static Tramite Map(this TramiteEditDto value)
        {
            if (value == null)
                return null;

            return new Tramite
            {
                JurisdiccionId = value.JurisdiccionId,
                JurisdiccionPlazoId = value.PlazoId
            };
        }
        public static IList<Tramite> Map(this IList<TramiteEditDto> value)
        {
            if (value == null)
                return new List<Tramite>();

            return value.Select(x => x.Map()).ToList();
        }
        public static Tramite Map(this TramiteEditDto value, Tramite item)
        {
            if (value == null)
                return null;

            item.Solicitud.MotivoConsulta = value.MotivoConsulta.TrimSafe();
            item.Solicitud.DatosAdicionales = value.DatosAdicionales.TrimSafe();
            item.Solicitud.Solicitante = value.Solicitante.Map(item.Solicitud.Solicitante);
            item.Solicitud.Entidad = value.Entidad.Map(item.Solicitud.Entidad);

            return item;
        }
        public static string ParseObservaciones(this TramiteEditDto value)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(value.ObservacionSolicitante))
            {
                sb.AppendLine(value.ObservacionSolicitante.Trim());
            }

            if (!string.IsNullOrEmpty(value.ObservacionEntidad))
            {
                sb.AppendLine(value.ObservacionEntidad.Trim());
            }

            if (value.Entidad.Inmueble != null)
            {
                foreach (var tomoFolio in value.Entidad.Inmueble.TomosFolios)
                {
                    if (!string.IsNullOrEmpty(tomoFolio.Observacion))
                    {
                        sb.AppendLine(tomoFolio.Observacion.Trim());
                    }
                }

                foreach (var persona in value.Entidad.Inmueble.Personas)
                {
                    if (!string.IsNullOrEmpty(persona.Observacion))
                    {
                        sb.AppendLine(persona.Observacion.Trim());
                    }
                }
            }

            return !string.IsNullOrEmpty(sb.ToString()) ? sb.ToString() : null;
        }
        public static TramiteResumenDto MapResumen(this Tramite value, IList<Feriado> feriadosNacionales)
        {
            if (value == null)
                return null;

            var model = new TramiteResumenDto
            {
                Id = value.Id,
                Numero = value.Solicitud.Numero,
                FechaTramite = value.GetFechaTramite().ToShort(),
                EstadoSolicitudDescripcion = value.Solicitud.TipoEstado.Descripcion,
                EstadoDescripcion = value.SubTipoEstado.TipoEstado.Descripcion,
                SubEstadoDescripcion = value.GetUltimoEstado().SubTipo.Descripcion,
                SubTipoEstadoId = value.GetUltimoEstado().SubTipoId,
                JurisdiccionInformante = value.Jurisdiccion.Sigla,
                JurisdiccionRequiriente = value.Solicitud.JurisdiccionRequirente.Sigla,
                ServicioDescripcion = value.Solicitud.Servicio.Descripcion,
                PlazoDescripcion = value.JurisdiccionPlazo.Plazo.Descripcion,
                TasaProvincial = value.GetCostoProvincial(),
                TasaNacional = value.Solicitud.GetCostoNacional(),

            };

            SetVencimiento(value, model, feriadosNacionales);

            return model;
        }

        private static void SetVencimiento(Tramite value, TramiteResumenDto model, IList<Feriado> feriadosNacionales)
        {
            var rangosSuspensiones = FeriadoHelper.GetRangosSuspensiones(value);
            var diasCursados = FeriadoHelper.GetDiasTranscurridos(value, feriadosNacionales, rangosSuspensiones);
            var fechaEntrega = FeriadoHelper.GetFechaEntrega(value.GetFechaTramite(), value.JurisdiccionPlazo.PlazoDias,
                                                             value.Jurisdiccion.Feriados, feriadosNacionales, rangosSuspensiones);

            model.DiasCursados = diasCursados;
            model.FechaEntrega = fechaEntrega.ToShort();
            model.Estado = GetEstadoVencimiento(fechaEntrega);

            var ultimoEstado = value.GetUltimoEstado();

            if (ultimoEstado.SubTipo.Identificador == SubTipoEstadoTramiteIdentificador.DocumentacionEnTramite)
            {
                model.FechaEntrega = Resources.Etiqueta_FechaEntregaDocEnTramite;
                model.Estado = GetEstadoVencimiento(fechaEntrega, ultimoEstado.Fecha);
            }
            else if (ultimoEstado.SubTipo.Identificador == SubTipoEstadoTramiteIdentificador.Observado)
            {
                model.FechaEntrega = Resources.Etiqueta_FechaEntregaObservado;
                model.Estado = GetEstadoVencimiento(fechaEntrega, ultimoEstado.Fecha);
            }
            else if (ultimoEstado.SubTipo.Identificador == SubTipoEstadoTramiteIdentificador.SeVerifico)
                model.Estado = GetEstadoVencimiento(fechaEntrega, ultimoEstado.Fecha);
            else if (ultimoEstado.SubTipo.Identificador == SubTipoEstadoTramiteIdentificador.Entregado)
            {
                var verificadoEstado = value.Estados.Where(x => x.SubTipo.Identificador == SubTipoEstadoTramiteIdentificador.SeVerifico)
                                                    .OrderBy(r => r.Id).Last();

                model.Estado = GetEstadoVencimiento(fechaEntrega, verificadoEstado.Fecha);
            }
        }

        private static TramiteEstadoVencimiento GetEstadoVencimiento(DateTime fechaEntrega)
        {
            if (fechaEntrega.Date == DateTime.Now.Date)
                return TramiteEstadoVencimiento.VenceHoy;

            if (fechaEntrega.Date < DateTime.Now.Date)
                return TramiteEstadoVencimiento.Vencido;

            return TramiteEstadoVencimiento.Vigente;
        }

        private static TramiteEstadoVencimiento GetEstadoVencimiento(DateTime fechaEntrega, DateTime fechaEstado)
        {
            if (fechaEstado.Date == fechaEntrega.Date)
                return TramiteEstadoVencimiento.VenceHoy;

            if (fechaEstado.Date > fechaEntrega.Date)
                return TramiteEstadoVencimiento.Vencido;

            return TramiteEstadoVencimiento.Vigente;
        }

        public static Boolean IsRectificacion(this TramiteEditDto value)
        {
            if (value == null)
                return false;

            return value.EsRectificacionInmueble || value.EsRectificacionPersona || value.EsRectificacionSolicitante;
        }

        public static TramiteResumenAdminEnteDto MapResumenAdmin(this Tramite value, List<Feriado> feriadosNacionales)
        {
            if (value == null)
                return null;

            var model = new TramiteResumenAdminEnteDto
            {
                Id = value.Id,
                Numero = value.Solicitud.Numero,
                FechaTramite = value.Solicitud.Fecha.ToString("d"),
                FechaMaxEntrega = GetFechaValidez(value.Solicitud, feriadosNacionales).ToString("d"),
                FechaEstado = value.Estados.Any() ? value.Estados.OrderByDescending(e => e.Fecha).First().Fecha.ToString("d") : String.Empty,
                EstadoSolicitud = value.Solicitud.TipoEstado.Descripcion,
                Estado = value.Estados.Any() ? value.Estados.OrderByDescending(e => e.Fecha).First().SubTipo.TipoEstado.Descripcion : String.Empty,
                SubEstado = value.Estados.Any() ? value.Estados.OrderByDescending(e => e.Fecha).First().SubTipo.Descripcion : String.Empty,
                JurisdiccionRequirente = $"{value.Solicitud.JurisdiccionRequirente.Descripcion} - {value.Solicitud.JurisdiccionRequirente.Sigla}",
                JurisdiccionInformante = $"{value.Jurisdiccion.Descripcion} - {value.Jurisdiccion.Sigla}"
            };

            return model;
        }

        private static DateTime GetFechaValidez(Solicitud solicitud, List<Feriado> feriadosNacionales)
        {
            return FeriadoHelper.GetFechaValidez(solicitud, feriadosNacionales);
        }

    }
}
