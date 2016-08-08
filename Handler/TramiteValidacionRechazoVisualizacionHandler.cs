using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using FluentValidation;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions.Validation;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteValidacionRechazoVisualizacionHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<TramiteValidacionRechazoVisualizacionDto> _validator;

        public TramiteValidacionRechazoVisualizacionHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio, 
                                                            IValidator<TramiteValidacionRechazoVisualizacionDto> validator) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _validator = validator;
        }

        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new TramiteValidacionRechazoVisualizacionDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            return new List<SubTipoEstadoTramiteIdentificador> { SubTipoEstadoTramiteIdentificador.ElPdfNoSeVisualiza };
        }
    }
}
