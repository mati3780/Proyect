using System.Linq;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class InmuebleExtension
    {
        public static InmuebleDto Map(this Inmueble value)
        {
            if (value == null)
                return null;

            var personas = value.Personas.ToList();

            var model = new InmuebleDto
                        {
                            Id = value.Id,
                            UbicacionInmueble = value.UbicacionInmueble,
                            Matricula = value.Matricula,
                            Zona = value.Zona,
                            UnidadComplementaria = value.UnidadComplementaria,
                            Lote = value.Lote,
                            Manzana = value.Manzana,
                            Legajo = value.Legajo,
                            NomenclaturaCatastralCircunscripcion = value.NomenclaturaCatastralCircunscripcion,
                            NomenclaturaCatastralSeccion = value.NomenclaturaCatastralSeccion,
                            NomenclaturaCatastralManzana = value.NomenclaturaCatastralManzana,
                            NomenclaturaCatastralParcela = value.NomenclaturaCatastralParcela,
                            Superficie = value.Superficie,
                            UnidadMedidaId = value.UnidadMedida?.Id ?? 0,
                            UnidadMedidaDescripcion = value.UnidadMedida != null ? value.UnidadMedida.Descripcion : string.Empty,
                            TomosFolios = value.TomoFolios.Map(),
                            PersonasFisicas = personas.Where(x => x.IsFisica()).ToList().Map(),
                            PersonasJuridicas = personas.Where(x => !x.IsFisica()).ToList().Map()
                        };

            return model;
        }
        public static Inmueble Map(this InmuebleDto value, Inmueble item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new Inmueble();
            
            item.UbicacionInmueble = value.UbicacionInmueble.TrimSafe();
            item.Matricula = value.Matricula.TrimSafe();
            item.Zona = value.Zona.TrimSafe();
            item.UnidadComplementaria = value.UnidadComplementaria.TrimSafe();
            item.Lote = value.Lote.TrimSafe();
            item.Manzana = value.Manzana.TrimSafe();
            item.Legajo = value.Legajo.TrimSafe();
            item.NomenclaturaCatastralCircunscripcion = value.NomenclaturaCatastralCircunscripcion.TrimSafe();
            item.NomenclaturaCatastralSeccion = value.NomenclaturaCatastralSeccion.TrimSafe();
            item.NomenclaturaCatastralManzana = value.NomenclaturaCatastralManzana.TrimSafe();
            item.NomenclaturaCatastralParcela = value.NomenclaturaCatastralParcela.TrimSafe();
            item.Superficie = value.Superficie;
            item.UnidadMedidaId = value.UnidadMedidaId > 0 ? value.UnidadMedidaId: null;
            item.TomoFolios = value.TomosFolios.Map(item.TomoFolios);
            item.Personas = value.Personas.Map(item.Personas);
            return item;
        }
    }
}
