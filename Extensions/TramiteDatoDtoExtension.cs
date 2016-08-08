using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class TramiteDatoDtoExtension
    {
        public static TramiteDatoDto Map(this JurisdiccionServicioDato value)
        {
            var model = new TramiteDatoDto
            {
                Id = value.TramiteDato.Id,
                Label = value.Label,
                Condicion = value.Condicion,
				CondicionDescripcion = Enum.GetName(typeof(TramiteDatoCondicion), value.Condicion),
				Nombre = value.TramiteDato.Nombre
            };

            return model;
        }
        public static TramiteDatoDto Map(this TramiteDato value)
        {
            var model = new TramiteDatoDto
            {
                Id = value.Id,
                Nombre = value.Nombre,
				Condicion = null
            };

            return model;
        }
        public static JurisdiccionServicioDato Map(this TramiteDatoDto value)
        {
            var item = new JurisdiccionServicioDato
                       {
                           Condicion = value.Condicion.Value,
                           Label = value.Label,
                           TramiteDatoId = value.Id,
                           JurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId()
            };

            return item;
        }
        public static IList<JurisdiccionServicioDato> Map(this TramiteDatoListDto value)
        {
            var datosList = value.PersonaFisicaDatos.Select(item => item.Value).ToList();
            datosList.AddRange(value.PersonaJuridicaDatos.Select(item => item.Value).ToList());
            datosList.AddRange(value.InmuebleDatos.Select(item => item.Value).ToList());

            return datosList.Select(dato => dato.Map()).ToList();
        }

        public static JurisdiccionServicioDato Map(this TramiteDatoDto value, JurisdiccionServicioDato item)
        {
            if (item == null)
                item = new JurisdiccionServicioDato();

            item.Label = value.Label;
            item.Condicion = value.Condicion.Value;

            return item;
        }
        public static List<JurisdiccionServicioDato> Map(this TramiteDatoListDto value, List<JurisdiccionServicioDato> items)
        {
            if (items == null)
                items = new List<JurisdiccionServicioDato>();

            var datosList = new List<TramiteDatoDto>();
            datosList.AddRange(value.PersonaFisicaDatos.Select(x=>x.Value).ToList());
            datosList.AddRange(value.PersonaJuridicaDatos.Select(x => x.Value).ToList());
            datosList.AddRange(value.InmuebleDatos.Select(x => x.Value).ToList());

            foreach (var datoDto in datosList)
            {
                var item = items.Find(x => x.TramiteDato.Id == datoDto.Id);

                if (item == null)
                    continue;

                datoDto.Map(item);
            }

            return items;
        }
        public static TramiteDatoListDto Map(this IList<TramiteDato> value)
        {
            var itemList = new TramiteDatoListDto();

            foreach (var item in value.Where(x => x.Entidad == TramiteDatoEntidad.PersonaFisica).ToList())
                itemList.PersonaFisicaDatos.Add(item.Nombre, item.Map());

            foreach (var item in value.Where(x => x.Entidad == TramiteDatoEntidad.PersonaJuridica).ToList())
                itemList.PersonaJuridicaDatos.Add(item.Nombre, item.Map());

            foreach (var item in value.Where(x => x.Entidad == TramiteDatoEntidad.Inmueble).ToList())
                itemList.InmuebleDatos.Add(item.Nombre, item.Map());

            return itemList;
        }
        public static TramiteDatoListDto Map(this IList<JurisdiccionServicioDato> value, Servicio servicio)
        {
            var itemList = new TramiteDatoListDto
                           {
                               ServicioId = servicio.Id,
							   ServicioDescripcion = servicio.Descripcion,
                               Inmueble = servicio.Inmueble
                           };

            foreach (var item in value.Where(x => x.TramiteDato.Entidad == TramiteDatoEntidad.PersonaFisica).ToList())
                itemList.PersonaFisicaDatos.Add(item.TramiteDato.Nombre, item.Map());

            foreach (var item in value.Where(x => x.TramiteDato.Entidad == TramiteDatoEntidad.PersonaJuridica).ToList())
                itemList.PersonaJuridicaDatos.Add(item.TramiteDato.Nombre, item.Map());

            foreach (var item in value.Where(x => x.TramiteDato.Entidad == TramiteDatoEntidad.Inmueble).ToList())
                itemList.InmuebleDatos.Add(item.TramiteDato.Nombre, item.Map());

            return itemList;
        }
        public static TramiteDatoListDto Merge(this TramiteDatoListDto value, IList<TramiteDato> datos)
        {
                var items = datos.Map();
                foreach (var dato in items.PersonaFisicaDatos)
                value.PersonaFisicaDatos.Add(dato);
                foreach (var dato in items.PersonaJuridicaDatos)
                    value.PersonaJuridicaDatos.Add(dato);
                foreach (var dato in items.InmuebleDatos)
                value.InmuebleDatos.Add(dato);

            return value;
 ;       }
        public static IList<TramiteDatoDto> GetList(this TramiteDatoListDto value)
        {
            var datosList = new List<TramiteDatoDto>();
            datosList.AddRange(value.PersonaFisicaDatos.Select(x=>x.Value).ToList());
            datosList.AddRange(value.PersonaJuridicaDatos.Select(x => x.Value).ToList());
            datosList.AddRange(value.InmuebleDatos.Select(x => x.Value).ToList());

            return datosList.ToList();
        }
    }
}
