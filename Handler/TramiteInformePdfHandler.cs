using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions.Common;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteInformePdfHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<InformacionTramiteDto> _validator;
        private readonly IRepositorio<Documento> _documentoRepositorio;

        public TramiteInformePdfHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio, IRepositorio<Documento> documentoRepositorio,
                                        IValidator<InformacionTramiteDto> validator) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _documentoRepositorio = documentoRepositorio;
            _validator = validator;
        }

        public override void Set(Tramite tramite, ITramiteActionDto dto, Byte[] pdfFile)
        {
            #region

            //var path = HttpContext.Current.Server.MapPath(@"~/Manual PROYECT v. 1.pdf");

            //MemoryStream memStream = new MemoryStream();
            //using (FileStream fileStream = File.OpenRead(path))
            //{

            //    memStream.SetLength(fileStream.Length);
            //    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            //}

            #endregion

            var file = _documentoRepositorio.GetAll().SingleOrDefault(x => x.TramiteId == dto.TramiteId);

            if (file != null)
                _documentoRepositorio.Delete(file);

            _documentoRepositorio.Add(new Documento
            {
                TramiteId = dto.TramiteId,
                Archivo = pdfFile
            });


            base.Set(tramite, dto, pdfFile);
        }

        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new InformacionTramiteDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            return new List<SubTipoEstadoTramiteIdentificador> { SubTipoEstadoTramiteIdentificador.SeRegistroPdf };
        }
    }
}
