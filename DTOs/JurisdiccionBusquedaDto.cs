using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECT.WebAPI.DTOs
{
    public class JurisdiccionBusquedaDto
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string Jurisdiccion { get; set; }
        public int? ProvinciaId { get; set; }
        public int? TipoEstadoAdhesionId { get; set; }
    }
}