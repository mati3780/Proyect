using System;
using System.Globalization;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using System.Linq;
using System.Net.Configuration;
using System.Security.Claims;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class PlazoValidator : AbstractValidator<JurisdiccionServicioPlazoDto>
    {
        private readonly IRepositorio<JurisdiccionServicioPlazo> _jurisdiccionServicioPlazoRepositorio;
        public PlazoValidator(IRepositorio<JurisdiccionServicioPlazo> jurisdiccionServicioPlazoRepositorio)
        {
            _jurisdiccionServicioPlazoRepositorio = jurisdiccionServicioPlazoRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            //GET
            RuleSet(RuleSetValidation.Obtener.ToString(), () => {
                RuleFor(x => x.Id).Must(PuedeObtener).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeObtener, "el Plazo del Trámite"));
            });

            //NEW
            RuleSet(RuleSetValidation.New.ToString(), () =>
            {
                RuleFor(x => x.VigenciaDesde.ToNuleableDate())
                    .NotEmpty().WithMessage(String.Format(Resources.Validacion_ValidDate, "Vigencia Desde"))
                    .GreaterThanOrEqualTo(DateTime.Today).WithMessage(String.Format(Resources.Validacion_GreaterThanOrEqualTo, "Vigencia Desde", "hoy"));

                RuleFor(x => x).Must(VigenciaDesdeDentroPeriodoDelTipoDeTramite).WithMessage(Resources.Validacion_PlazoVigenciaDesdeDentroDePeriodoInvalido);
            });

            //EDIT
            RuleSet(RuleSetValidation.Edit.ToString(), () =>
            {
                RuleFor(x => x.Id).Must(PuedeObtener).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeObtener, "el Plazo del Trámite"));

                RuleFor(x => x.EntidadVigenciaHasta)
                    .Empty().WithMessage(Resources.Validacion_PlazoCerrado);

                RuleFor(x => x.VigenciaHasta.ToNuleableDate())
                    .NotEmpty().WithMessage(String.Format(Resources.Validacion_ValidDate, "Vigencia Hasta"))
                    .GreaterThanOrEqualTo(DateTime.Today).WithMessage(String.Format(Resources.Validacion_GreaterThanOrEqualTo, "Vigencia Hasta", "hoy"));

                RuleFor(x => x)
                    .Must(VigenciaHastaEsGreaterThanVigenciaDesde).WithMessage(String.Format(Resources.Validacion_GreaterThan, "Vigencia Hasta", "Vigencia Desde"));
            });
        }
        #region Private Methods
        private bool PuedeObtener(Int64 id)
        {
            var result = _jurisdiccionServicioPlazoRepositorio.Get(id);

            if (ClaimsPrincipal.Current.IsAdmin())
                return true;

            return result != null && result.Jurisdiccion.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }
        private bool PuedeInsertar(Int64 id)
        {
            return id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }
        private bool VigenciaDesdeDentroPeriodoDelTipoDeTramite(JurisdiccionServicioPlazoDto model)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
            var vigenciaDesde = model.VigenciaDesde.ToNuleableDate();
            
            return !_jurisdiccionServicioPlazoRepositorio.GetAll()
                   .Any(x => x.JurisdiccionId == jurisdiccionId && x.ServicioId == model.ServicioId
                   && x.PlazoId == model.PlazoId && (!x.VigenciaHasta.HasValue || x.VigenciaHasta.Value >= vigenciaDesde));
        }

        private bool VigenciaHastaEsGreaterThanVigenciaDesde(JurisdiccionServicioPlazoDto model)
        {
            return model.VigenciaHasta.ToNuleableDate() > model.EntidadVigenciaDesde;
        }
        #endregion
    }
}