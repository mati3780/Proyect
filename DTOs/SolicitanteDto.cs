using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitanteDto
    {
        public Int64 Id { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Domicilio { get; set; }
        public String CodigoPostal { get; set; }
        public String Telefono { get; set; }
        public String TelefonoMovil { get; set; }
        public Int64 NumeroDocumento { get; set; }
        public String Email1 { get; set; }
        public String Email2 { get; set; }
        public Int64 Cuit { get; set; }
        public String Matricula { get; set; }
        public String Tomo { get; set; }
        public String Folio { get; set; }
        public String DatosMatriculacion { get; set; }
        public Int64 LocalidadId { get; set; }
        public Int64 TipoDocumentoId { get; set; }
        public Int64 ProfesionId { get; set; }
        public LocalidadDto Localidad { get; set; }
        public Int64 ProvinciaId { get; set; }
        public ProvinciaDto Provincia { get; set; }
        public TipoDocumentoDto TipoDocumento { get; set; }
        public ProfesionDto Profesion { get; set; }
    }
}
