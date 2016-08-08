using System;
using PROYECT.Dominio.Contracts.Dtos;

namespace PROYECT.WebAPI.DTOs
{
    public class BusquedaDto : IBusquedaDto
    {
        public Int32 Start { get; set; }
        public Int32 Length { get; set; }
        public Boolean Asc { get; set; }
        public String SortBy { get; set; }
    }
}