using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteConsultaDto
    {
        public Int64 Id { get; set; }
        public String JurisdiccionDescripcion { get; set; }
        public String ProvinciaDescripcion { get; set; }
        public String PlazoDescripcion { get; set; }
        public String EstadoComentario { get; set; }
    }
}