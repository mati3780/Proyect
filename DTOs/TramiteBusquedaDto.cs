using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteBusquedaDto : BusquedaDto
    {
        public String NumeroTramite { get; set; }
        public Int16? TipoDocumento { get; set; }
        public Int64? NumeroDocumento { get; set; }
        public Int64? EstadoSolicitud { get; set; }
        public Int64? EstadoTramite { get; set; }
        public Int64? SubEstadoTramite { get; set; }
    }
}
