using System;

namespace PROYECT.WebAPI.DTOs
{
    public class ProfesionDto
    {
        public Int64 Id { get; set; }
        public String Descripcion { get; set; }
        public Int16? Orden { get; set; }
        public Boolean RequiereMatricula { get; set; }
    }
}
