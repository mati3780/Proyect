using System;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitanteRectificacionDto
    {
        public Int64 SolicitanteId { get; set; }
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
        public String LocalidadDescripcion { get; set; }
        public Int64 TipoDocumentoId { get; set; }
        public String TipoDocumentoDescripcion { get; set; }
        public Int64 ProfesionId { get; set; }
        public String ProfesionDescripcion { get; set; }
        public DateTime Fecha { get; set; }
        public String Observacion { get; set; }
    }
}