using System;

namespace PROYECT.WebAPI.DTOs
{
    public class PagoDto
    {
        public Int64 Id { get; set; }
        public String Fecha { get; set; }
        public String Entidad { get; set; }
        public String Sucursal { get; set; }
        public String CodigoIdentificacion { get; set; }
        public SolicitudDto Solicitud { get; set; }
    }
}
