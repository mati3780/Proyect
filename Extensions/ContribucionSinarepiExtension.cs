using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.Dominio.Extensions.Common;

namespace PROYECT.WebAPI.Extensions
{
    public static class ContribucionProyectExtension
    {
        public static ContribucionProyectDto Map(this TasaNacional value)
        {
            if (value == null)
                return null;

            var model = new ContribucionProyectDto
            {
                Id = value.Id,
                FechaDesde = value.FechaDesde.ToShort(),
                FechaHasta = value.FechaHasta?.ToShort(),
                Valor = value.Costo
            };

            return model;
        }
        public static TasaNacional Map(this ContribucionProyectDto value, TasaNacional item)
        {
            if (item == null)
                item = new TasaNacional();
            
            item.FechaDesde = value.FechaDesde.ToDate();
            item.Costo = value.Valor;
            
            return item;
        }
        public static TasaNacional MapSimple(this ContribucionProyectDto value, TasaNacional item)
        {
            
            item.FechaHasta = value.FechaHasta.ToDate();
            return item;
        }
        
    }
}
