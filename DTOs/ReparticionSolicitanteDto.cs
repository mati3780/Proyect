using System;

namespace PROYECT.WebAPI.DTOs
{
    public class ReparticionSolicitanteDto
    {
        public Int64 Id { get; set; }
        public String Descripcion { get; set; }
        public Boolean TasaNacional { get; set; }
        public Boolean TasaProvincial { get; set; }
    }
}
