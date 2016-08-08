using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Http.ModelBinding;
using FluentValidation;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Email;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteDocumentacionEnTramiteHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<DocumentacionEnTramiteDto> _validator;

        public TramiteDocumentacionEnTramiteHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio, 
                                                    IValidator<DocumentacionEnTramiteDto> validator) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _validator = validator;
        }

        public override void SendEmail(Tramite tramite)
        {
            var jurisdiccionRequirente = tramite.Solicitud.JurisdiccionRequirente;

            var html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailDocumentacionTramite.html"))
                                        .Replace("[[NroSolicitud]]", tramite.Solicitud.Numero)
                                        .Replace("[[NombreJurisdiccionRequirente]]", jurisdiccionRequirente.Descripcion);

            MailSender.SendEmail(new[] { tramite.Solicitud.Solicitante.Email1, tramite.Solicitud.Solicitante.Email2 },
                                 String.Format(Resources.Etiqueta_AvisoInformeEstadoTramite, tramite.Solicitud.Numero), html, null);
        }

        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new DocumentacionEnTramiteDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            return new List<SubTipoEstadoTramiteIdentificador> { SubTipoEstadoTramiteIdentificador.DocumentacionEnTramite };
        }
    }
}
