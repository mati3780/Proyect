﻿using System;
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
using PROYECT.WebAPI.Helpers;

namespace PROYECT.WebAPI.Handler
{
    public class TramiteEntregaAceptacionHandler : TramiteHandlerBase, ITramiteActionHandler
    {
        private readonly IValidator<TramiteEntregaAceptacionDto> _validator;

        public TramiteEntregaAceptacionHandler(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio, 
                                               IValidator<TramiteEntregaAceptacionDto> validator) : base(subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _validator = validator;
        }

        public override bool Valid(ITramiteActionDto dto, Object modelState)
        {
            var model = new TramiteEntregaAceptacionDto { TramiteId = dto.TramiteId, Observacion = dto.Observacion };
            return _validator.IsValid(model, (ModelStateDictionary)modelState, RuleSetValidation.Default);
        }

        public override IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite)
        {
            return new List<SubTipoEstadoTramiteIdentificador> { SubTipoEstadoTramiteIdentificador.Entregado };
        }
    }
}