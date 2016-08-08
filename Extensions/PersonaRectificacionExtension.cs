using System;
using System.Collections.Generic;
using System.Linq;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    public static class PersonaRectificacionExtension
    {
        public static IList<PersonaRectificacion> MapRectificacion(this IList<Persona> value, IRectificacionDto dto)
        {
            if (!value.Any())
                return new List<PersonaRectificacion>();

            var personas = new List<PersonaRectificacion>();
            foreach (var item in dto.Entidad.Inmueble.PersonasFisicas.Where(x => x.Borrado).ToList())
            {
                var val = value.SingleOrDefault(x => x.Id == item.Id);
                if (val != null)
                {
                    var persona = val.MapRectificacion();
                    item.Observacion = persona.Observacion;
                    personas.Add(persona);
                }
            }

            foreach (var item in dto.Entidad.Inmueble.PersonasJuridicas.Where(x => x.Borrado).ToList())
            {
                var val = value.SingleOrDefault(x => x.Id == item.Id);
                if (val != null)
                {
                    var persona = val.MapRectificacion();
                    persona.Observacion = item.Observacion;
                    personas.Add(persona);
                }
            }

            return personas;
        }

        public static IList<Persona> MapPersonasFisicas(this IList<PersonaDto> value, IList<Persona> items)
        {
            var results = new List<Persona>();

            if (!value.Any())
                return results;

            foreach (var persona in value)
            {
                var entidad = items.SingleOrDefault(x => x.Id == persona.Id);

                if (entidad != null && persona.Borrado)
                    items.Remove(entidad);
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

        public static PersonaRectificacion MapRectificacion(this Persona value)
        {
            if (value == null)
                return null;

            PersonaRectificacion item;

            if (value.IsFisica())
            {
                item = new PersonaFisicaRectificacion()
                       {
                           Cuit = value.Cuit,
                           Fecha = DateTime.Now,
                           Apellido = ((PersonaFisica) value).Apellido,
                           ApellidoMaterno = ((PersonaFisica) value).ApellidoMaterno,
                           Nombre = ((PersonaFisica) value).Nombre,
                           NumeroDocumento = ((PersonaFisica) value).NumeroDocumento,
                           TipoDocumentoId = ((PersonaFisica) value).TipoDocumentoId,
                           TipoDocumentoDescripcion = ((PersonaFisica) value).TipoDocumento.Descripcion
                       };
            }
            else
            {
                item = new PersonaJuridicaRectificacion()
                       {
                           Cuit = value.Cuit,
                           Fecha = DateTime.Now,
                           Domicilio = ((PersonaJuridica) value).Domicilio,
                           RazonSocial = ((PersonaJuridica) value).RazonSocial,
                           SociedadIGJId = ((PersonaJuridica) value).SociedadIGJId,
                           Folio = ((PersonaJuridica) value).Folio,
                           Tomo = ((PersonaJuridica) value).Tomo
                       };
            }

            return item;
        }
        public static PersonaRectificacionDto Map(this PersonaRectificacion value)
        {
            var model = new PersonaRectificacionDto
            {
                Cuit = value.Cuit,
                Fecha = value.Fecha
            };

            if (value.IsFisica())
            {
                var persona = (PersonaFisicaRectificacion)value;

                model.Nombre = persona.Nombre;
                model.Apellido = persona.Apellido;
                model.ApellidoMaterno = persona.ApellidoMaterno;
                model.NumeroDocumento = persona.NumeroDocumento;
                model.TipoDocumentoId = persona.TipoDocumentoId;
                model.TipoDocumentoDescripcion = persona.TipoDocumentoDescripcion;
            }
            else
            {
                var persona = (PersonaJuridicaRectificacion)value;
                model.Domicilio = persona.Domicilio;
                model.Folio = persona.Folio;
                model.Tomo = persona.Tomo;
                model.RazonSocial = persona.RazonSocial;
                model.SociedadIGJId = persona.SociedadIGJId;
            }

            return model;
        }
        public static PersonaRectificacion Map(this PersonaRectificacionDto value, PersonaRectificacion item)
        {
            if (value.IsFisica)
            {
                if (item == null)
                    item = new PersonaFisicaRectificacion();

                var persona = ((PersonaFisicaRectificacion) item);
                persona.PersonaId = value.PersonaId;
                persona.Cuit = value.Cuit;
                persona.Apellido = value.Apellido;
                persona.ApellidoMaterno = value.ApellidoMaterno;
                persona.Nombre = value.Nombre;
                persona.NumeroDocumento = value.NumeroDocumento;
                persona.TipoDocumentoId = value.TipoDocumentoId;
                persona.TipoDocumentoDescripcion = value.TipoDocumentoDescripcion;
            }
            else
            {
                if (item == null)
                    item = new PersonaJuridicaRectificacion();

                var persona = ((PersonaJuridicaRectificacion)item);
                persona.PersonaId = value.PersonaId;
                persona.Cuit = value.Cuit;
                persona.Domicilio = value.Domicilio;
                persona.RazonSocial = value.RazonSocial;
                persona.SociedadIGJId = value.SociedadIGJId;
                persona.Folio = value.Folio;
                persona.Tomo = value.Tomo;
            }

            return item;
        }
    }
}
