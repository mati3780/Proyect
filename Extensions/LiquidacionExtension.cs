using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Helper.Repositorios.Helpers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class LiquidacionExtension
    {
        public static LiquidacionDto Map(this Liquidacion value, long jurisdiccionId, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            if (value == null)
                return null;

            var model = new LiquidacionDto
            {
                Id = value.Id,
                Movimiento = value.GetMovimiento(jurisdiccionesEnte).GetDescription(),
                MovimientoEnumValue = (int)value.GetMovimiento(jurisdiccionesEnte),
                CuentasBancarias = value.Destino.CuentasBancarias.Where(x => !x.FechaVigencia.HasValue || x.FechaVigencia.Value >= DateTime.Today).OrderByDescending(x => x.Principal).ThenBy(x => x.Entidad).ThenBy(x => x.Sucursal).Select(x => x.Map()).ToList(),
                Movimientos = value.Detalles.OrderBy(x => x.FechaTramite).Select(x => x.Map(jurisdiccionesEnte)).ToList(),
                FechaLimiteDeposito = value.Fecha.AddDays(AppConfigHelper.LiquidacionPlazoDeposito).ToShort(),
                FechaCorteHasta = value.FechaCorteHasta.ToShortDateString(),
                Origen = !string.IsNullOrEmpty(value.Origen.Sigla) ? value.Origen.Sigla : value.Origen.Descripcion,
                Destino = !string.IsNullOrEmpty(value.Destino.Sigla) ? value.Destino.Sigla : value.Destino.Descripcion,
                Importe = string.Format("$ {0}", value.Importe),
                ExisteConciliacion = value.Conciliacion != null,
                EstadoDeposito = value.Origen.IsBloqueado() ? string.Format("Bloqueado {0}", value.Origen.EstadosAdhesion.Last().Fecha.ToShortDateString()) : string.Empty
            };

            if (value.Deposito != null)
            {
                model.ExisteDeposito = true;
                model.CuentaBancariaId = value.Deposito.CuentaId;
                model.FechaDeposito = value.Deposito.Fecha.ToShortDateString();
                model.Transaccion = value.Deposito.Transaccion;
                model.Entidad = value.Deposito.Entidad;
                model.Sucursal = value.Deposito.Sucursal;
                model.Cajero = value.Deposito.Cajero;
                model.CuentaBancaria = value.Deposito.Cuenta.Map();
                model.ExistePdfDeposito = value.Deposito.Recibo != null;
            }

            if (value.Compensacion != null)
            {
                model.FechaCompensacion = value.Compensacion.Fecha.ToShortDateString();
            }

            if (value.Conciliacion != null)
            {
                model.ConciliacionVerificada = value.Conciliacion.Verificada.ToString();
                model.ConciliacionVerificacion = value.Conciliacion.Verificada;
                model.ConciliacionObservacion = value.Conciliacion.Observacion;
                model.ExistePdfConciliacion = value.Conciliacion.Recibo != null;
            }

            return model;
        }
        public static LiquidacionListItemDto MapListItem(this Liquidacion value, Int64 jurisdiccionId, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            if (value == null)
                return null;

            var model = new LiquidacionListItemDto
            {
                Id = value.Id,
                FechaLimiteDeposito = value.Fecha.AddHours(AppConfigHelper.LiquidacionPlazoDeposito).ToShort(),
                FechaCorteHasta = value.Fecha.ToShortDateString(),
                Movimiento = value.GetMovimiento(jurisdiccionesEnte).GetDescription(),
                Origen = !string.IsNullOrEmpty(value.Origen.Sigla) ? value.Origen.Sigla : value.Origen.Descripcion,
                Destino = !string.IsNullOrEmpty(value.Destino.Sigla) ? value.Destino.Sigla : value.Destino.Descripcion,
                Importe = Math.Round(value.Importe, 2),
                FechaDeposito = value.Deposito?.Fecha.ToShortDateString() ?? String.Empty,
                FechaCompensacion = value.Compensacion?.Fecha.ToShortDateString() ?? String.Empty,
                ImporteCompensacion = value.ImporteCompensado,
                ConciliacionBancaria = value.Conciliacion != null ? "SI" : "NO",
                Estado = value.Origen.IsBloqueado() ? string.Format("Bloqueado {0}", value.Origen.EstadosAdhesion.Last().Fecha.ToShortDateString()) : string.Empty,
                EstadoSemaforo = GetEstadoVencimiento(value.Fecha.AddHours(AppConfigHelper.LiquidacionPlazoDeposito)),
            };

            //TODO: Ver jurisdiccion de Origen, para ver si puede registrar.
            model.PuedeRegistrar =
                (model.Movimiento == "Depósito" && value.OrigenId == jurisdiccionId && value.Deposito == null && value.Compensacion == null)
                || (model.Movimiento == "Acreditación" && value.DestinoId == jurisdiccionId && value.Deposito != null && value.Conciliacion == null && value.Compensacion == null)
                || (value.OrigenId == jurisdiccionId && value.DestinoId == jurisdiccionId && (value.Deposito == null || value.Conciliacion == null && value.Compensacion == null));//CASO ENTE - ENTE

            return model;
        }
        public static Liquidacion MapDeposito(this LiquidacionDto value, Liquidacion item)
        {
            if (item.Deposito == null)
                item.Deposito = new Deposito();

            item.Deposito.Fecha = value.FechaDeposito.ToDate();
            item.Deposito.Transaccion = value.Transaccion;
            item.Deposito.Entidad = value.Entidad;
            item.Deposito.Sucursal = value.Sucursal;
            item.Deposito.Cajero = value.Cajero;
            item.Deposito.CuentaId = value.CuentaBancariaId.Value;

            return item;
        }
        public static Liquidacion MapAcreditacion(this LiquidacionDto value, Liquidacion item)
        {
            if (item.Conciliacion == null)
                item.Conciliacion = new Conciliacion();

            item.Conciliacion.Fecha = DateTime.Now;
            item.Conciliacion.Verificada = bool.Parse(value.ConciliacionVerificada);
            item.Conciliacion.Observacion = value.ConciliacionObservacion;

            return item;
        }
        public static TipoLiquidacion GetMovimiento(this Liquidacion value, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            var enteOrigen = jurisdiccionesEnte.Any(x => x.Id == value.Origen.Id);

            if ((enteOrigen))
            {
                if (ClaimsPrincipal.Current.IsEnte() || ClaimsPrincipal.Current.IsAdmin())
                    return TipoLiquidacion.Deposito;

                return TipoLiquidacion.Acreditacion;
            }

            if (ClaimsPrincipal.Current.IsEnte() || ClaimsPrincipal.Current.IsAdmin())
                return TipoLiquidacion.Acreditacion;

            return TipoLiquidacion.Deposito;
        }
        private static int GetEstadoVencimiento(DateTime fechaDeposito)
        {
            if (fechaDeposito.Date == DateTime.Now.Date)
                return (int)TramiteEstadoVencimiento.VenceHoy;

            if (fechaDeposito.Date < DateTime.Now.Date)
                return (int)TramiteEstadoVencimiento.Vencido;

            return (int)TramiteEstadoVencimiento.Vigente;
        }
        public static LiquidacionDetalleDto Map(this LiquidacionDetalle value, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            if (value == null)
                return null;

            var model = new LiquidacionDetalleDto
            {
                Id = value.Id,
                FechaTramite = value.FechaTramite.ToShortDateString(),
                NumeroTramite = value.NumeroTramite,
                TasaNacional = value.TasaNacional,
                TasaProvincial = value.TasaProvincial,
                ImporteProyect = value.ImporteProyect
            };

            if (ClaimsPrincipal.Current.IsEnte() || ClaimsPrincipal.Current.IsAdmin())
            {
                if (value.Informante)
                {
                    model.Debito = value.Importe;
                    model.Movimiento = TipoLiquidacion.Deposito.GetDescription();
                }
                else
                {
                    model.Credito = value.Importe;
                    model.Movimiento = TipoLiquidacion.Acreditacion.GetDescription();
                }
            }
            else
            {
                if (value.Informante)
                {
                    model.Credito = value.Importe;
                    model.Movimiento = TipoLiquidacion.Acreditacion.GetDescription();
                }
                else
                {
                    model.Debito = value.Importe;
                    model.Movimiento = TipoLiquidacion.Deposito.GetDescription();
                }
            }

            return model;
        }
        public static LiquidacionResumenDto MapResumen(this Liquidacion value, IList<Jurisdiccion> jurisdiccionesEnte)
        {
            var movimiento = value.GetMovimiento(jurisdiccionesEnte);
            var model = new LiquidacionResumenDto
            {
                Id = value.Id,
                Fecha = value.Fecha.ToShort(),
                Movimiento = movimiento.GetDescription(),
                MovimientoEnumValue = (int)movimiento,
                Origen = !string.IsNullOrEmpty(value.Origen.Sigla) ? value.Origen.Sigla : value.Origen.Descripcion,
                Destino = !string.IsNullOrEmpty(value.Destino.Sigla) ? value.Destino.Sigla : value.Destino.Descripcion,
                Importe = string.Format("$ {0}", value.Importe)
            };

            return model;
        }

    }
}