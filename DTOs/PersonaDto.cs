using System;

namespace PROYECT.WebAPI.DTOs
{
    public class PersonaDto
    {
        public Int64 Id { get; set; }
        public Int64? Cuit { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String ApellidoMaterno { get; set; }
        public Int64? NumeroDocumento { get; set; }
        public Int64? TipoDocumentoId { get; set; }
        public TipoDocumentoDto TipoDocumento { get; set; }
        public Boolean IsFisica { get; set; }
        public String RazonSocial { get; set; }
        public String SociedadIGJId { get; set; }
        public String Domicilio { get; set; }
        public String Tomo { get; set; }
        public String Folio { get; set; }
        public String Observacion { get; set; }
        public Boolean Borrado { get; set; }
    }
}
