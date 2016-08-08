using System;
using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class CompensacionExtension
    {
        public static CompensacionResumenDto MapResumen(this Compensacion value)
        {
            if (value == null)
                return null;

            var model = new CompensacionResumenDto
            {
                Id = value.Id,
                Importe = value.Importe,
                Fecha = value.Fecha.ToShort(),
                Jurisdiccion = value.Jurisdiccion.Sigla + " - " + value.Jurisdiccion.Provincia.Descripcion,
                FechaDeposito = value.Deposito?.Fecha.ToShort()
            };

            return model;
        }
        public static CompensacionDto Map(this Compensacion value)
        {
            if (value == null)
                return null;

            var model = new CompensacionDto
            {
                //TODO
            };

            return model;
        }
        public static Compensacion Map(this CompensacionDto value, Compensacion item, List<Liquidacion> liquidaciones)
        {
            if (value == null)
                return null;

            //lista Acreditaciones - suma Acreditaciones 
            
            var sumaAcreditaciones = liquidaciones.Where(x => x.DestinoId == value.EnteId).Sum(x => x.Importe);
            
            foreach (var acreditacion in liquidaciones.Where(x => x.DestinoId == value.EnteId).OrderBy(x => x.Fecha))
            {
                acreditacion.ImporteCompensado = acreditacion.Importe;
            }

            var liquidacionesDepositos = liquidaciones.Where(x => x.DestinoId != value.EnteId && x.OrigenId == value.EnteId).OrderBy(x => x.Importe);
            var sumaDepositos = liquidacionesDepositos.Sum(x => x.Importe);

            if (sumaDepositos != sumaAcreditaciones)
            {
                //Datos del depósito de la compensacion
                item.Deposito = new Deposito
                {
                    Fecha = Convert.ToDateTime(value.FechaDeposito),
                    Transaccion = value.Transaccion,
                    Entidad = value.Entidad,
                    Sucursal = value.Sucursal,
                    Cajero = value.Cajero,
                    CuentaId = value.CuentaBancariaId.Value
                };
                //Fin datos deposito
            }
                

            foreach (var deposito in liquidacionesDepositos)
            {
                if (deposito != liquidacionesDepositos.Last())
                {
                    deposito.ImporteCompensado = deposito.Importe;
                    sumaAcreditaciones -= deposito.Importe;
                }
                else
                {
                    deposito.ImporteCompensado = Math.Abs(sumaAcreditaciones - deposito.Importe);
                    if(sumaDepositos != sumaAcreditaciones)
                        deposito.Deposito = item.Deposito;
                }
            }
            
            //Datos Compensacion
            item.Importe = CalcularTotal(liquidaciones, value.EnteId);
            item.Fecha = DateTime.Now;
            item.Jurisdiccion = liquidaciones.First().Origen.Id != value.EnteId ? liquidaciones.First().Origen : liquidaciones.First().Destino;
            item.Liquidaciones = liquidaciones;

            return item;
        }
        private static decimal CalcularTotal(List<Liquidacion> liquidaciones, long jurisdiccionDestino)
        {
            decimal total = 0;
            foreach (var l in liquidaciones)
            {
                if (l.DestinoId == jurisdiccionDestino)
                {
                    total += l.Importe;
                }
                else
                    total -= l.Importe;
            }
            return Math.Abs(total);
        }
    }
}
