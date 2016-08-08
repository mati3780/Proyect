using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitanteExtension
    {
        public static SolicitanteDto Map(this Solicitante value)
        {
            if (value == null)
                return null;

            var model = new SolicitanteDto
            {
                Id = value.Id,
                Nombre = value.Nombre,
                Apellido = value.Apellido,
                NumeroDocumento = value.NumeroDocumento,
                Cuit = value.Cuit,
                Domicilio = value.Domicilio,
                CodigoPostal = value.CodigoPostal,
                Telefono = value.Telefono,
                TelefonoMovil = value.TelefonoMovil,
                Email1 = value.Email1,
                Email2 = value.Email2,
                Matricula = value.Matricula,
                Tomo = value.Tomo,
                Folio = value.Folio,
                DatosMatriculacion = value.DatosMatriculacion,
                TipoDocumentoId = value.TipoDocumentoId,
                LocalidadId = value.LocalidadId,
                ProvinciaId = value.Localidad.Municipio.Provincia.Id,
                ProfesionId = value.ProfesionId,
                TipoDocumento = value.TipoDocumento.Map(),
                Localidad = value.Localidad.Map(),
                Provincia = value.Localidad.Municipio.Provincia.Map(),
                Profesion = value.Profesion.Map()
            };

            return model;
        }
        public static Solicitante Map(this SolicitanteDto value, Solicitante item)
        {
            if (value == null)
                return null;

            if (item == null)
                item = new Solicitante();
            
            item.LocalidadId = value.LocalidadId;
            item.Nombre = value.Nombre.TrimSafe();
            item.Apellido = value.Apellido.TrimSafe();
            item.Cuit = value.Cuit;
            item.CodigoPostal = value.CodigoPostal.TrimSafe();
            item.DatosMatriculacion = value.DatosMatriculacion.TrimSafe();
            item.Domicilio = value.Domicilio.TrimSafe();
            item.NumeroDocumento = value.NumeroDocumento;
            item.Telefono = value.Telefono.TrimSafe();
            item.TelefonoMovil = value.TelefonoMovil.TrimSafe();
            item.Email1 = value.Email1.TrimSafe();
            item.Email2 = value.Email2.TrimSafe();
            item.Matricula = value.Matricula.TrimSafe();
            item.Tomo = value.Tomo.TrimSafe();
            item.Folio = value.Folio.TrimSafe();
            item.TipoDocumentoId = value.TipoDocumentoId;
            item.LocalidadId = value.LocalidadId;
            item.ProfesionId = value.ProfesionId;

            return item;
        }
    }
}
