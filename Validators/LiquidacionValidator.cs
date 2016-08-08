using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
    public class LiquidacionValidator : AbstractValidator<LiquidacionDto>
    {
        private readonly IRepositorio<Liquidacion> _liquidacionRepositorio;

        public LiquidacionValidator(IRepositorio<Liquidacion> liquidacionRepositorio)
        {
            _liquidacionRepositorio = liquidacionRepositorio;
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Deposito.ToString(), () => {
                RuleFor(x => x.FechaDeposito).NotEmpty().Must(ValidateFecha).WithMessage(String.Format(Resources.Validacion_ValidDate, "Fecha Depósito"));

                RuleFor(x => x.FechaDeposito.ToDate())
                    .GreaterThanOrEqualTo(x => x.FechaCorteHasta.ToDate()).WithMessage(String.Format(Resources.Validacion_GreaterThanOrEqualTo, "Fecha Depósito", "Límite para Depositar"));

                RuleFor(x => x.FechaDeposito).Must(x => x.ToDate() <= DateTime.Now).WithMessage(String.Format(Resources.Validacion_LessThanOrEqualTo, "Fecha Depósito", "Hoy"));

                RuleFor(x => x.CuentaBancariaId).NotEmpty();
            });

            RuleSet(RuleSetValidation.Acreditacion.ToString(), () => {
                RuleFor(x => x.ConciliacionVerificada).NotEmpty().Must(ValidateBool).WithMessage(String.Format(Resources.Validacion_Invalid, ""));
            });
        }

        private static bool ValidateFecha(String value)
        {
            var result = value.ToNuleableDate();
            return result != null;
        }

        private static bool ValidateBool(String value)
        {
            bool result;
            return Boolean.TryParse(value, out result);
        }
    }
}