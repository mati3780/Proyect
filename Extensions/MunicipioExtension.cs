using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class MunicipioExtension
    {
        public static MunicipioDto Map(this Municipio value)
        {
            var model = new MunicipioDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
    }
}
