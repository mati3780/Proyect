using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class ServicioExtension
    {
		public static ServicioDto Map(this Servicio value)
		{
			if (value == null)
				return null;

			var model = new ServicioDto
			{
				Id = value.Id,
				Descripcion = value.Descripcion,
				Inmueble = value.Inmueble,
				Orden = value.Orden
			};

			return model;
		}

		public static ServicioDto MapSimple(this Servicio value)
        {
            return new ServicioDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Inmueble = value.Inmueble,
                Orden = value.Orden
            };
        }

    }
}