using System.Linq;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class TipoEstadoJurisdiccionExtensions
    {
        public static TipoEstadoAdhesionDto Map(this TipoEstadoAdhesion value)
        {
            var model = new TipoEstadoAdhesionDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden
            };

            return model;
        }
    }
}