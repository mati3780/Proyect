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
    public class FeriadoValidator : AbstractValidator<FeriadoDto>
    {
        private readonly IRepositorio<Feriado> _feriadoRepositorio;
        private readonly IRepositorio<JurisdiccionServicioPlazo> _jurisdiccionServicioPlazoRepositorio;

        public FeriadoValidator(IRepositorio<Feriado> feriadoRepositorio, IRepositorio<JurisdiccionServicioPlazo> jurisdiccionServicioPlazoRepositorio)
        {
            _feriadoRepositorio = feriadoRepositorio;
            _jurisdiccionServicioPlazoRepositorio = jurisdiccionServicioPlazoRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Obtener.ToString(), () => {
                RuleFor(x => x.Id).Must(PuedeObtener).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeObtener, "el feriado"));
            });

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                RuleFor(x => x.Fecha).NotEmpty().Must(ValidateFecha);
                RuleFor(x => x.Descripcion).NotEmpty().Length(1, 250);
                RuleFor(x => x).Must(Exists).WithMessage(Resources.Validacion_RegistroYaExiste);
                RuleFor(x => x).Must(ValidarFechaMinimaIngresada).WithMessage(Resources.Validacion_FechaMinima);
            });
            
            RuleSet(RuleSetValidation.Edit.ToString(), () => {
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeHacerCambios, "el feriado"));
                RuleFor(x => x).Must(ValidarRegistroEditable).WithMessage(Resources.Validacion_FechaNoEditable);
            });

            RuleSet(RuleSetValidation.Delete.ToString(), () => {
                RuleFor(x => x).Must(ValidarPropiedad).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeEliminar, "el feriado"));
                RuleFor(x => x).Must(ValidarRegistroEditable).WithMessage(Resources.Validacion_FechaNoEditable);
            });
        }
        
        #region Private Methods

        private bool Exists(FeriadoDto item)
        {
            var fecha = item.Fecha.ToNuleableDate();

            if (fecha == null)
                return true;

            var query = _feriadoRepositorio.GetAll().Where(x => x.Fecha == fecha && x.Id != item.Id);

	        if (ClaimsPrincipal.Current.IsAdmin())
		        query = query.Where(x => x.Jurisdiccion == null);
	        else
	        {
		        var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
				query = query.Where(x => x.Jurisdiccion.Id == jurisdiccionId || x.Jurisdiccion == null);
			}
            
            return !query.Any();
        }

        private bool PuedeObtener(Int64 id)
        {
            var result = _feriadoRepositorio.Get(id);

            if (ClaimsPrincipal.Current.IsAdmin())
                return true;

            return !result.JurisdiccionId.HasValue || result.Jurisdiccion.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }

        private bool ValidarPropiedad(FeriadoDto item)
        {
            var result = _feriadoRepositorio.Get(item.Id);

            if (ClaimsPrincipal.Current.IsAdmin())
                return result.Jurisdiccion == null;
            
            return result.JurisdiccionId.HasValue && result.Jurisdiccion.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }

        private static bool ValidateFecha(String value)
        {
            var result = value.ToNuleableDate();
            return result != null;
        }

        private bool ValidarFechaMinimaIngresada(FeriadoDto item)
        {
            return ValidarFechaMinima(item.Fecha.ToString());
        }
        private bool ValidarFechaMinima(string value)
        {
            var fecha = value.ToNuleableDate();

            if (fecha == null)
                return true;

            return fecha > DateTime.Today;
        }
        private bool ValidarRegistroEditable(FeriadoDto item)
        {
            if (!ValidarPropiedad(item)) return true;
            var result = _feriadoRepositorio.Get(item.Id);
            return ValidarFechaMinima(result.Fecha.ToString());
        }
        #endregion
    }
}