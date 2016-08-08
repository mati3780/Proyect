using System;

namespace PROYECT.WebAPI.DTOs
{
    public class CuentaBancariaDto
    {
        public Int64 Id { get; set; }
        public String Entidad { get; set; }
        public String Sucursal { get; set; }
        public Int64 TipoId { get; set; }
        public string Tipo { get; set; }
        public String Numero { get; set; }
        public String CBU { get; set; }
        public string FechaVigencia { get; set; }
        public Boolean Principal { get; set; }
        public string Jurisdiccion { get; set; }
    }
}