using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http.ModelBinding;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Helpers.Email;
using PROYECT.Helpers.Pdf;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteVerificacionPdfHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<TramiteVerificacionDto> _validator;
        private readonly IRepositorio<Documento> _documentoRepositorio;

        public TramiteVerificacionPdfHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio,
                                             IValidator<TramiteVerificacionDto> validator,
                                             IRepositorio<Documento> documentoRepositorio) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _validator = validator;
            _documentoRepositorio = documentoRepositorio;
        }

        public override void SendEmail(Tramite tramite)
        {
            var jurisdiccionRequirente = tramite.Solicitud.JurisdiccionRequirente;
            var jurisdiccionInformante = tramite.Jurisdiccion;
            var documento = _documentoRepositorio.GetAll().SingleOrDefault(d => d.TramiteId == tramite.Id);

            string html;
            bool valido = false;

            if (documento != null)
                valido = PdfHelper.HasValidSignature(documento.Archivo);

            if (valido)
                html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailEntregaTramite.html"));
            else
                html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailRetiroTramite.html"))
                                                        .Replace("[[DireccionJurisdiccionRequirente]]", $"{jurisdiccionRequirente.Direccion} - {jurisdiccionRequirente.Localidad.Descripcion}")
                                                        .Replace("[[ProvinciaJurisdiccionRequirente]]", jurisdiccionRequirente.Provincia.Descripcion)
                                                        .Replace("[[UrlJurisdiccionRequirente]]", jurisdiccionRequirente.Url);

            html = html.Replace("[[NroSolicitud]]", tramite.Solicitud.Numero)
                       .Replace("[[NombreJurisdiccionInformante]]", jurisdiccionInformante.Descripcion)
                       .Replace("[[ProvinciaJurisdiccionInformante]]", jurisdiccionInformante.Provincia.Descripcion)
                       .Replace("[[NombreJurisdiccionRequirente]]", jurisdiccionRequirente.Descripcion)
                       .Replace("[[UrlJurisdiccionInformante]]", jurisdiccionInformante.Url);


            MailSender.SendEmail(new[] { tramite.Solicitud.Solicitante.Email1, tramite.Solicitud.Solicitante.Email2 },
                    String.Format(valido ? Resources.Etiqueta_AvisoEntregaTramite : Resources.Etiqueta_AvisoRetiroTramite, tramite.Solicitud.Numero), html, valido ? new[] { new Attachment("Solicitud.pdf", documento.Archivo, "application/pdf") } : null);
        }
        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new TramiteVerificacionDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            var documento = _documentoRepositorio.GetAll().SingleOrDefault(d => d.TramiteId == tramite.Id);

            var estados = new List<SubTipoEstadoTramiteIdentificador> {SubTipoEstadoTramiteIdentificador.SeVerifico};

            if (documento != null && PdfHelper.HasValidSignature(documento.Archivo))
                estados.Add(SubTipoEstadoTramiteIdentificador.Entregado);

            return estados;
        }
    }
}
