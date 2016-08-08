using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class LocalidadExtension
    {
        public static LocalidadDto Map(this Localidad value)
        {
            var model = new LocalidadDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
    }
}
