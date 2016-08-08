using System;
using System.Collections.Generic;
using PROYECT.Dominio.Enums;

namespace PROYECT.WebAPI.DTOs
{
    public class LiquidacionDto
    {
        public LiquidacionDto()
        {
            LiquidacionesCompensacion = new List<LiquidacionListItemDto>();
            CuentasBancarias = new List<CuentaBancariaDto>();
            Movimientos = new List<LiquidacionDetalleDto>();
        }
        public Int64 Id { get; set; }
        public string Movimiento { get; set; }
        public int MovimientoEnumValue { get; set; }
        public Int64? CuentaBancariaId { get; set; }
        public CuentaBancariaDto CuentaBancaria { get; set; }
        public List<LiquidacionListItemDto> LiquidacionesCompensacion { get; set; }
        public List<CuentaBancariaDto> CuentasBancarias { get; set; }
        public List<LiquidacionDetalleDto> Movimientos { get; set; }
        public string FechaLimiteDeposito { get; set; }
        public string FechaCorteHasta { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Importe { get; set; }
        public string FechaDeposito { get; set; }
        public string FechaCompensacion { get; set; }
        public string Transaccion { get; set; }
        public string Entidad { get; set; }
        public string Sucursal { get; set; }
        public string Cajero { get; set; }
        public bool ExisteDeposito { get; set; }
        public string EstadoDeposito { get; set; }
        public string EstadoAcreditacion { get; set; }
        public bool ExisteConciliacion { get; set; }
        public string ConciliacionVerificada { get; set; }
        public bool ConciliacionVerificacion { get; set; }
        public string ConciliacionObservacion { get; set; }
        public bool ExistePdfConciliacion { get; set; }
        public bool ExistePdfDeposito { get; set; }
    }

    public class LiquidacionListItemDto
    {
        public Int64 Id { get; set; }
        public string FechaLimiteDeposito { get; set; }
        public string FechaCorteHasta { get; set; }
        public string Movimiento { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public decimal Importe { get; set; }
        public string FechaDeposito { get; set; }
        public string FechaCompensacion { get; set; }
        public decimal? ImporteCompensacion { get; set; }
        public string ConciliacionBancaria { get; set; }
        public string Estado { get; set; }
        public int EstadoSemaforo { get; set; }
        public bool PuedeRegistrar { get; set; }
    }
}