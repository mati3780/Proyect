using System;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    public static class InmuebleRectificacionExtension
    {
        public static InmuebleRectificacion MapRectificacion(this Inmueble value, IRectificacionDto dto)
        {
            if (value == null)
                return null;

            var item = new InmuebleRectificacion
            {
                InmuebleId = value.Id,
                UbicacionInmueble = value.UbicacionInmueble,
                Matricula = value.Matricula,
                Zona = value.Zona,
                UnidadComplementaria = value.UnidadComplementaria,
                Lote = value.Lote,
                Manzana = value.Manzana,
                Legajo = value.Legajo,
                NomenclaturaCatrastalCiscunscripcion = value.NomenclaturaCatastralCircunscripcion,
                NomeclaturaCatastralSeccion = value.NomenclaturaCatastralSeccion,
                NomenclaturaCatastralManzana = value.NomenclaturaCatastralManzana,
                NomenclaturaCatastralParcela = value.NomenclaturaCatastralParcela,
                Superficie = value.Superficie,
                UnidadMedida = value.UnidadMedida?.Descripcion,
                Personas = value.Personas.MapRectificacion(dto),
                TomoFolios = value.TomoFolios.MapRectificacion(dto),
                Fecha = DateTime.Now
            };

            return item;
        }
        public static InmuebleRectificacionDto Map(this InmuebleRectificacion value)
        {
            var model = new InmuebleRectificacionDto
            {
                InmuebleId = value.InmuebleId,
                UbicacionInmueble = value.UbicacionInmueble,
                Matricula = value.Matricula,
                Zona = value.Zona,
                UnidadComplementaria = value.UnidadComplementaria,
                Lote = value.Lote,
                Manzana = value.Manzana,
                Legajo = value.Legajo,
                NomenclaturaCatrastalCiscunscripcion = value.NomenclaturaCatrastalCiscunscripcion,
                NomeclaturaCatastralSeccion = value.NomeclaturaCatastralSeccion,
                NomenclaturaCatastralManzana = value.NomenclaturaCatastralManzana,
                NomenclaturaCatastralParcela = value.NomenclaturaCatastralParcela,
                Superficie = value.Superficie,
                UnidadMedida = value.UnidadMedida
            };

            return model;
        }
    }
}
