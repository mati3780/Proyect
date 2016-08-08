using System;

namespace PROYECT.WebAPI.DTOs
{
    public class TomoFolioDto
    {
        public Int64 Id { get; set; }
        public String Tomo { get; set; }
        public String Folio { get; set; }
        public String Observacion { get; set; }
        public Boolean Borrado { get; set; }
    }
}