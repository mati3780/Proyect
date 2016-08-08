using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROYECT.WebAPI.DTOs
{
    public class CompensacionDto
    {
        public CompensacionDto()
        {
            LiquidacionesIds = new List<long>();
            Liquidaciones = new List<LiquidacionListItemDto>();
        }
        public Int64 Id { get; set; }
        public Int64? CuentaBancariaId { get; set; }
        public CuentaBancariaDto CuentaBancaria { get; set; }
        public List<long> LiquidacionesIds { get; set; }
        public List<LiquidacionListItemDto> Liquidaciones { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public decimal Importe { get; set; }
        public string FechaDeposito { get; set; }
        public DateTime FechaCompensacion { get; set; }
        public string Transaccion { get; set; }
        public string Entidad { get; set; }
        public string Sucursal { get; set; }
        public string Cajero { get; set; }
        public long JurisdiccionDestino { get; set; }
        public long EnteId { get; set; }
    }
}
