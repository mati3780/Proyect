using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudResumenDto
    {
        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String FechaTramite { get; set; }
        public String ServicioDescripcion { get; set; }
        public Decimal TasaProvincial { get; set; }
        public Decimal TasaNacional { get; set; }
    }
}
