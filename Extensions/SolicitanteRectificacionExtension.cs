using System;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.Extensions
{
    public static class SolicitanteRectificacionExtension
    {
        public static SolicitanteRectificacion MapRectificacion(this Solicitante value, IRectificacionDto dto)
        {
            if (value == null)
                return null;

            var item = new SolicitanteRectificacion
            {
                Nombre = value.Nombre,
                Apellido = value.Apellido,
                Cuit = value.Cuit,
                CodigoPostal = value.CodigoPostal,
                DatosMatriculacion = value.DatosMatriculacion,
                Domicilio = value.Domicilio,
                NumeroDocumento = value.NumeroDocumento,
                Telefono = value.Telefono,
                TelefonoMovil = value.TelefonoMovil,
                Email1 = value.Email1,
                Email2 = value.Email2,
                Matricula = value.Matricula,
                Tomo = value.Tomo,
                Folio = value.Folio,
                LocalidadId = value.LocalidadId,
                LocalidadDescripcion = value.Localidad.Descripcion,
                ProfesionId = value.ProfesionId,
                TipoDocumentoId = value.TipoDocumentoId,
                ProfesionDescripcion = value.Profesion.Descripcion,
                TipoDocumentoDescripcion = value.TipoDocumento.Descripcion,
                SolicitanteId = value.Id,
                Fecha = DateTime.Now,
                Observacion = dto.ObservacionSolicitante
            };

            return item;
        }
        public static SolicitanteRectificacionDto Map(this SolicitanteRectificacion value)
        {
            var model = new SolicitanteRectificacionDto
            {
                SolicitanteId = value.SolicitanteId,
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
                LocalidadId = value.LocalidadId,
                LocalidadDescripcion = value.LocalidadDescripcion,
                Fecha = value.Fecha,
                Observacion = value.Observacion
            };

            return model;
        }
    }
}
