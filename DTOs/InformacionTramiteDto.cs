using System;
using PROYECT.Dominio.Contracts.Dtos;

namespace PROYECT.WebAPI.DTOs
{
    public class InformacionTramiteDto : ITramiteActionDto
    {
        public Int64 TramiteId { get; set; }
        public string Observacion { get; set; }
    }
}
