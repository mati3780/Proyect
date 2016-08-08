using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Http.ModelBinding;
using FluentValidation;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Email;
using PROYECT.Helpers.Pdf;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteObservacionHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<TramiteObservacionDto> _validator;

        public TramiteObservacionHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio,
                                         IValidator<TramiteObservacionDto> validator) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _validator = validator;
        }

        public override void SendEmail(Tramite tramite)
        {
            var jurisdiccionRequirente = tramite.Solicitud.JurisdiccionRequirente;
            var jurisdiccionInformante = tramite.Jurisdiccion;
            byte[] pdf;

            if (tramite.Solicitud.Entidad.Persona != null)
                pdf = tramite.Solicitud.Entidad.Persona.IsFisica()
                    ? PdfHelper.GetPdfTemplatePersonaHumana(tramite)
                    : PdfHelper.GetPdfTemplatePersonaJuridica(tramite);
            else
                pdf = PdfHelper.GetPdfTemplateInmueble(tramite);

            var html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailObservacionTramite.html"))
                                                        .Replace("[[NroSolicitud]]", tramite.Solicitud.Numero)
                                                        .Replace("[[ApellidoNombreSolicitante]]", $"{tramite.Solicitud.Solicitante.Apellido}, {tramite.Solicitud.Solicitante.Nombre}")
                                                        .Replace("[[FechaActual]]", DateTime.Now.ToString("d"))
                                                        .Replace("[[NombreJurisdiccionInformante]]", jurisdiccionInformante.Descripcion)
                                                        .Replace("[[ProvinciaJurisdiccionInformante]]", jurisdiccionInformante.Provincia.Descripcion)
                                                        .Replace("[[JuridiccionRequirente]]", $"{jurisdiccionRequirente.Descripcion}, {jurisdiccionRequirente.Direccion}, {jurisdiccionRequirente.Localidad.Descripcion} - {jurisdiccionRequirente.Provincia.Descripcion}")
                                                        .Replace("[[ObservacionTramite]]", tramite.GetUltimoEstado().Observacion);

            MailSender.SendEmail(new[] { tramite.Solicitud.Solicitante.Email1, tramite.Solicitud.Solicitante.Email2 },
                                 String.Format(Resources.Etiqueta_AvisoObservacionTramite, tramite.Solicitud.Numero),
                                 html,
                                 new[] { new Attachment("Solicitud.pdf", pdf, "application/pdf") });
        }

        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new TramiteObservacionDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            return new List<SubTipoEstadoTramiteIdentificador> { SubTipoEstadoTramiteIdentificador.Observado };
        }
    }
}
