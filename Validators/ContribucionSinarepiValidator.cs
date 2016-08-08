using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class ContribucionProyectValidator : AbstractValidator<ContribucionProyectDto>
    {
        private readonly IRepositorio<TasaNacional> _contribucionProyectRepositorio;

        public ContribucionProyectValidator(IRepositorio<TasaNacional> contribucionProyectRepositorio)
        {
            _contribucionProyectRepositorio = contribucionProyectRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                RuleFor(x => x.Valor).NotEmpty();
            });

            RuleSet(RuleSetValidation.New.ToString(), () => {
                RuleFor(x => x).Must(ValidarPuedeCrear).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "la contribución PROYECT"));
                RuleFor(x => x.FechaDesde).NotEmpty().Must(ValidateFecha);
                RuleFor(x => x).Must(ExisteSuperposicion).WithMessage(Resources.Validacion_TasaNacionalVigenciaDesdeDentroDePeriodoInvalido);
            });

            RuleSet(RuleSetValidation.Edit.ToString(), () => {
                RuleFor(x => x).Must(ValidarPuedeEditar).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "la contribución PROYECT"));
                RuleFor(x => x.FechaHasta).NotEmpty().Must(ValidateFecha);
                RuleFor(x => x.FechaHasta.ToDate()).GreaterThanOrEqualTo(x => x.FechaDesde.ToDate()).WithMessage(String.Format(Resources.Validacion_GreaterThanOrEqualTo, "Fecha Hasta", "Fecha Desde"));
            });

        }
        
        #region Private Methods

        private bool ExisteSuperposicion(ContribucionProyectDto item)
        {
            DateTime fechaDesde = item.FechaDesde.ToDate();
            var query = _contribucionProyectRepositorio.GetAll().Where(x => x.FechaHasta != null && x.FechaHasta.Value >= fechaDesde);
            return !query.Any();
        }
        
        private bool ValidarPuedeCrear(ContribucionProyectDto item)
        {
            var query = _contribucionProyectRepositorio.GetAll().Where(x => x.FechaHasta == null);
            return !query.Any();
        }

        private bool ValidarPuedeEditar(ContribucionProyectDto item)
        {
            var result = _contribucionProyectRepositorio.Get(item.Id);
            return !result.FechaHasta.HasValue;
        }

        private static bool ValidateFecha(String value)
        {
            var result = value.ToDate();
            return result != null;
        }

        #endregion
    }
}