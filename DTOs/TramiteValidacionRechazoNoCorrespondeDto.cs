using System;
using PROYECT.Dominio.Contracts.Dtos;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteValidacionRechazoNoCorrespondeDto : ITramiteActionDto
    {
        public Int64 TramiteId { get; set; }
        public String Observacion { get; set; }
    }
}

