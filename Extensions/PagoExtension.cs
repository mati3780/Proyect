using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class PagoExtension
    {
        public static PagoDto Map(this Pago value)
        {
            if (value == null)
                return null;

            var model = new PagoDto
            {
                Id = value.Id,
                Fecha = value.Fecha.ToShort(),
                Entidad = value.Entidad,
                Sucursal = value.Sucursal,
                CodigoIdentificacion = value.CodigoIdentificacion.TrimSafe(),
                //Solicitud = value.Solicitud.Map() Comentado por elarosa, si se descomenta se produce redundancia ciclica al mapear el inmueble.
            };

            return model;
        }
        public static Pago Map(this PagoDto value, Pago item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new Pago();
            
            item.Fecha = value.Fecha.ToDate();
            item.Entidad = value.Entidad;
            item.Sucursal = value.Sucursal;
            item.CodigoIdentificacion = value.CodigoIdentificacion.TrimSafe();

            return item;
        }
    }
}
