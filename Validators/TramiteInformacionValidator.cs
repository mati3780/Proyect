using System;
using System.Linq;
using System.Security.Claims;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Contracts.Validators;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class TramiteInformacionValidator : AbstractValidator<InformacionTramiteDto>, ITramiteActionValidator
    {
        private readonly IRepositorio<Tramite> _tramiteRepositorio;

        public TramiteInformacionValidator(IRepositorio<Tramite> tramiteRepositorio)
        {
            _tramiteRepositorio = tramiteRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "el trámite"));
                RuleFor(x => x).Must(ValidarEstado).WithMessage(Resources.Validacion_EstadoTramiteInvalido);
            });
        }

        #region Private Methods

        private bool ValidarPropiedad(InformacionTramiteDto item)
        {
            var result = _tramiteRepositorio.Get(item.TramiteId);

            if (ClaimsPrincipal.Current.IsAdmin())
                return false;

            return result.Jurisdiccion.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }

        private bool ValidarEstado(InformacionTramiteDto item)
        {
            var tramite = _tramiteRepositorio.GetAll().Single(x => x.Id == item.TramiteId);
            return tramite.CanSubirInforme();
        }

        #endregion
    }
}