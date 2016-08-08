using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class PlazoExtension
    {
        public static SeleccionableDto MapSimple(this Plazo value)
        {
            if (value == null)
                return null;

            return new SeleccionableDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };
        }
    }
}