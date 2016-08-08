using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class ProvinciaExtension
    {
        public static ProvinciaDto Map(this Provincia value)
        {
            var model = new ProvinciaDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
    }
}