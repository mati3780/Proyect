using System;
using System.Security.Claims;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class TramiteValidator : AbstractValidator<TramiteEditDto>
    {
        private readonly IRepositorio<Tramite> _tramiteRepositorio;

        public TramiteValidator(IRepositorio<Tramite> tramiteRepositorio)
        {
            _tramiteRepositorio = tramiteRepositorio;
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () => {

            });

            RuleSet(RuleSetValidation.Edit.ToString(), () => {
                //Validar que la tasa nacional este vigente al momento de registrar el tramite
                //Validar el estado del trámite para ver si no esta anulado, finalizado y/o vencido
                //TODO
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "el trámite"));
            });
        }

        private bool ValidarPropiedad(TramiteEditDto item)
        {
            if (ClaimsPrincipal.Current.IsAdmin())
                return false;

            var result = _tramiteRepositorio.Get(item.Id);
            return result.Solicitud.JurisdiccionRequirente.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }
    }
}