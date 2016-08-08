using System;
using System.Linq;
using System.Security.Claims;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class CuentaBancariaValidator : AbstractValidator<CuentaBancariaDto>
    {
        private readonly IRepositorio<CuentaBancaria> _cuentaBancariaRepositorio;

        public CuentaBancariaValidator(IRepositorio<CuentaBancaria> cuentaBancariaRepositorio)
        {
            _cuentaBancariaRepositorio = cuentaBancariaRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Obtener.ToString(), () => {
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeObtener, "el CuentaBancaria"));
            });

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                RuleFor(x => x.Entidad).NotEmpty().Length(1, 100);
                RuleFor(x => x.Sucursal).NotEmpty().Length(1, 100);
                RuleFor(x => x.TipoId).NotEmpty();
                RuleFor(x => x.Numero).NotEmpty().Length(1, 40);
                RuleFor(x => x.CBU).NotEmpty().Matches(@"^\d{22}$");
                RuleFor(x => x).Must(Exists).WithMessage(Resources.Validacion_RegistroYaExiste);
            });
            
            RuleSet(RuleSetValidation.Edit.ToString(), () => {
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "la cuenta bancaria"));
                RuleFor(x => x.FechaVigencia).NotEmpty().Must(ValidateFecha);
            });
            
        }
        
        #region Private Methods

        private bool Exists(CuentaBancariaDto item)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
            var query = _cuentaBancariaRepositorio.GetAll().Where(x => x.JurisdiccionId == jurisdiccionId && x.CBU == item.CBU && x.Id != item.Id && (!x.FechaVigencia.HasValue || x.FechaVigencia >= DateTime.Today));
            return !query.Any();
        }
        
        private bool ValidarPropiedad(CuentaBancariaDto item)
        {
            var result = _cuentaBancariaRepositorio.Get(item.Id);
            return result.Jurisdiccion.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }

        private static bool ValidateFecha(String value)
        {
            var result = value.ToDate();
            return result != null;
        }

        #endregion
    }
}