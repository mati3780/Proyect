using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    static class UnidadMedidaExtension
    {
        public static SeleccionableDto Map(this UnidadMedida value)
        {
            var model = new SeleccionableDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
    }
}
