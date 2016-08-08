using System;
using System.Collections.Generic;

namespace PROYECT.WebAPI.DTOs
{
    
    public class JurisdiccionDto
    {
        public JurisdiccionDto()
        {
            CuentasBancarias = new List<CuentaBancariaDto>();
        }

        public Int64 Id { get; set; }
        public String Descripcion { get; set; }
        public String Sigla { get; set; }
        public String Autoridad { get; set; }
        public String Direccion { get; set; }
        public String CodigoPostal { get; set; }
        public String Telefono { get; set; }
        public String Email1 { get; set; }
        public String Email2 { get; set; }
        public Int64 Cuit { get; set; }
        public Int32? Orden { get; set; }
        public Boolean RecibirMail { get; set; }
        public String Url { get; set; }
        public String Observacion { get; set; }
        public Int16? PlazoObservacion { get; set; }
        public Int16? PlazoRetiroDocumentacion { get; set; }
        public Int64? LocalidadId { get; set; }
        public String LocalidadDescripcion { get; set; }
        public Int64 ProvinciaId { get; set; }
        public String ProvinciaDescripcion { get; set; }
        public Int64 TipoEstadoAdhesionId { get; set; }
        public String TipoEstadoAdhesionDescripcion { get; set; }
        public String GrupoAd { get; set; }
        public IList<CuentaBancariaDto> CuentasBancarias { get; set; }
    }
}
