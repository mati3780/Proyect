using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using System.Collections.Generic;
using System.Linq;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.WebAPI.Helpers;

namespace PROYECT.WebAPI.Extensions
{
    public static class PersonaExtension
    {
        public static IList<PersonaDto> Map(this IList<Persona> value)
        {
            if (!value.Any())
                return new List<PersonaDto>();

            return value.Select(x => x.Map()).ToList();
        }
        public static IList<Persona> Map(this IList<PersonaDto> value, IList<Persona> items)
        {
            var results = new List<Persona>();

            if (!value.Any())
                return results;

            foreach (var persona in value)
            {
                var entidad = items.SingleOrDefault(x => x.Id == persona.Id);

                if (entidad != null && persona.Borrado)
                {
                    var personaRepository = (IRepositorio<Persona>)ActivatorHelper.GetService(typeof (IRepositorio<Persona>));
                    items.Remove(entidad);
                    personaRepository.Delete(entidad);
                }
                else
                {
                    var result = persona.Map(entidad);
                    if (result.Id == 0)
                        results.Add(result);
                }
            }

            foreach (var r in results)
                items.Add(r);

            return items;
        }
        public static PersonaDto Map(this Persona value)
        {
            if (value == null)
                return null;

            var model = new PersonaDto
            {
                Id = value.Id,
                Cuit = value.Cuit,
                IsFisica = value.IsFisica()
            };

            if (model.IsFisica)
            {
                var fisica = (PersonaFisica)value;
                model.Nombre = fisica.Nombre;
                model.Apellido = fisica.Apellido;
                model.ApellidoMaterno = fisica.ApellidoMaterno;
                model.NumeroDocumento = fisica.NumeroDocumento;
                model.TipoDocumentoId = fisica.TipoDocumentoId;
                model.TipoDocumento = fisica.TipoDocumento?.Map();
            }
            else
            {
                var juridica = (PersonaJuridica)value;
                model.Domicilio = juridica.Domicilio;
                model.Folio = juridica.Folio;
                model.Tomo = juridica.Tomo;
                model.RazonSocial = juridica.RazonSocial;
                model.SociedadIGJId = juridica.SociedadIGJId;
            }

            return model;
        }
        public static Persona Map(this PersonaDto value, Persona item)
        {
            if (value == null)
                return null;

           return  value.IsFisica ? MapFisica(value, item) : MapJuridica(value, item);
        }
        public static Persona MapFisica(this PersonaDto value, Persona item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new PersonaFisica();

                item.Cuit = value.Cuit;
                ((PersonaFisica)item).Apellido = value.Apellido.TrimSafe();
                ((PersonaFisica)item).ApellidoMaterno = value.ApellidoMaterno.TrimSafe();
                ((PersonaFisica)item).Nombre = value.Nombre.TrimSafe();
                ((PersonaFisica)item).NumeroDocumento = value.NumeroDocumento;
                ((PersonaFisica)item).TipoDocumentoId = value.TipoDocumentoId;

            return item;
        }
        public static Persona MapJuridica(this PersonaDto value, Persona item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new PersonaJuridica();

            item.Cuit = value.Cuit;
            ((PersonaJuridica)item).Domicilio = value.Domicilio.TrimSafe();
            ((PersonaJuridica)item).RazonSocial = value.RazonSocial.TrimSafe();
            ((PersonaJuridica)item).SociedadIGJId = value.SociedadIGJId;
            ((PersonaJuridica)item).Folio = value.Folio.TrimSafe();
            ((PersonaJuridica)item).Tomo = value.Tomo.TrimSafe();

            return item;
        }
        public static TipoDocumentoDto Map(this TipoDocumento value)
        {
            if (value == null)
                return null;

            var model = new TipoDocumentoDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion
            };

            return model;
        }
        public static ProfesionDto Map(this Profesion value)
        {
            var model = new ProfesionDto
            {
                Id = value.Id,
                Descripcion = value.Descripcion,
                Orden = value.Orden,
                RequiereMatricula = value.RequiereMatricula
            };

            return model;
        }
    }
}
