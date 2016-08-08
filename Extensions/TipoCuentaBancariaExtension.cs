using System.Linq;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class TipoCuentaBancariaExtension
    {
        public static TipoCuentaBancariaDto Map(this TipoCuentaBancaria value)
        {
            var model = new TipoCuentaBancariaDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden
            };

            return model;
        }
    }
}