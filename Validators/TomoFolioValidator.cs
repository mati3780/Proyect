using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PROYECT.Dominio.Enums;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
	public class TomoFolioValidator : AbstractValidator<TomoFolioDto>
	{
		public TomoFolioValidator()
		{
			ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

			RuleSet(RuleSetValidation.Default.ToString(), () =>
			{
				RuleFor(x => x.Tomo).NotEmpty().WithMessage("El campo Tomo es obligatorio");
				RuleFor(x => x.Folio).NotEmpty().WithMessage("El campo Folio es obligatorio");

				RuleFor(x => x.Tomo).Length(1, 50).When(x => !string.IsNullOrWhiteSpace(x.Tomo)).WithMessage("La longitud máxima del campo Tomo es de 50 caracteres");
				RuleFor(x => x.Folio).Length(1, 50).When(x => !string.IsNullOrWhiteSpace(x.Folio)).WithMessage("La longitud máxima del campo Folio es de 50 caracteres");
			});
		}
	}
}