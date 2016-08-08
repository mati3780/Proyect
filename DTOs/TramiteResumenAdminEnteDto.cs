using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteResumenAdminEnteDto
    {
        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String FechaTramite { get; set; }
        public String FechaMaxEntrega { get; set; }
        public String FechaEstado { get; set; }
        public String EstadoSolicitud { get; set; }
        public String Estado { get; set; }
        public String SubEstado { get; set; }
        public String JurisdiccionRequirente { get; set; }
        public String JurisdiccionInformante { get; set; }
    }
}
