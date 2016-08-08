using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitudExtension
    {
        public static SolicitudDto Map(this Solicitud value)
        {
            if (value == null)
                return null;

            var model = new SolicitudDto
            {
                Id = value.Id,
                Numero = value.Numero,
                MotivoConsulta = value.MotivoConsulta,
                Observacion = value.Observacion,
                DatosAdicionales = value.DatosAdicionales,
                ServicioId = value.ServicioId,
                JurisdiccionRequirenteId = value.JurisdiccionRequirenteId,
                ReparticionSolicitanteId = value.ReparticionSolicitanteId,
                TasaNacionalId = value.TasaNacional?.Id ?? 0,
                JurisdiccionRequirente = value.JurisdiccionRequirente.MapSimple(),
                ReparticionSolicitante = value.ReparticionSolicitante.MapSimple(),
                Solicitante = value.Solicitante.Map(),
                Entidad = value.Entidad.Map(),
                Servicio = value.Servicio.Map(),
                Pago = value.Pago.Map(),
                Tramites = value.Tramites.MapDetalle(),
                FechaSolicitud = value.Fecha.ToShort()
            };

            return model;
        }
        public static SolicitudConsultaDTO MapConsulta(this Solicitud value, List<Feriado> feriadosNacionales)
        {
            if (value == null)
                return null;

            var model = new SolicitudConsultaDTO
            {
                Id = value.Id,
                Numero = value.Numero,
                EstadoComentario = value.TipoEstado.Identificador == TipoEstadoSolicitudIdentificador.PresentacionPendiente
                                                ? $"{value.TipoEstado.Comentario} - Fecha Limite de Presentación: {GetFechaValidez(value, feriadosNacionales).ToShortDateString()}"
                                                : value.TipoEstado.Comentario,
                JurisdiccionRequirente = value.JurisdiccionRequirente.Map(),
                ReparticionSolicitante = value.ReparticionSolicitante.Map(),
                Solicitante = value.Solicitante.Map(),
                Entidad = value.Entidad.Map(),
                Servicio = value.Servicio.Map(),
                Pago = value.Pago.Map(),
                Tramites = value.Tramites.MapConsulta()
            };

            return model;
        }

        public static SolicitudResumenDto MapResumen(this Solicitud value)
        {
            if (value == null)
                return null;

            var model = new SolicitudResumenDto
            {
                Id = value.Id,
                Numero = value.Numero,
                FechaTramite = value.Fecha.ToShort(),
                ServicioDescripcion = value.Servicio.Descripcion,
                TasaNacional = value.GetCostoNacional(),
                TasaProvincial = value.GetCostoProvincial()
            };

            return model;
        }
        public static Solicitud Map(this SolicitudDto value, Solicitud item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new Solicitud { Fecha = DateTime.Now };

            item.MotivoConsulta = value.MotivoConsulta.TrimSafe();
            item.DatosAdicionales = value.DatosAdicionales.TrimSafe();
            item.Observacion = value.Observacion.TrimSafe();
            item.ServicioId = value.ServicioId;
            item.Solicitante = value.Solicitante.Map(item.Solicitante);
            item.Pago = value.Pago.Map(item.Pago);
            item.JurisdiccionRequirenteId = value.JurisdiccionRequirenteId;
            item.ReparticionSolicitanteId = value.ReparticionSolicitanteId;
            item.Entidad = value.Entidad.Map(item.Entidad);

            if (!item.Tramites.Any())
            {
                foreach (var tramite in value.Tramites.MapDetalle())
                    item.AddJurisdiccion(tramite);
            }

            return item;
        }

        public static string ParseObservaciones(this SolicitudDto value)
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
        public static TipoEstadoTramiteDto Map(this TipoEstadoSolicitud value)
        {
            if (value == null)
                return null;

            var model = new TipoEstadoTramiteDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden
            };

            return model;
        }
        public static Boolean IsRectificacion(this SolicitudDto value)
        {
            if (value == null)
                return false;

            return value.EsRectificacionInmueble || value.EsRectificacionPersona || value.EsRectificacionSolicitante;
        }

        private static DateTime GetFechaValidez(Solicitud solicitud, List<Feriado> feriadosNacionales)
        {
            return FeriadoHelper.GetFechaValidez(solicitud, feriadosNacionales);
        }

    }
}
