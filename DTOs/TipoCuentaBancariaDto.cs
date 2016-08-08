using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TipoCuentaBancariaDto
    {
        public Int64 Id { get; set; }
        public String Descripcion { get; set; }
        public Int16? Orden { get; set; }
    }
}
