using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudBusquedaDto : BusquedaDto
    {
        public String NumeroTramite { get; set; }
        public Int16? TipoDocumento { get; set; }
        public Int64? NumeroDocumento { get; set; }
    }
}
