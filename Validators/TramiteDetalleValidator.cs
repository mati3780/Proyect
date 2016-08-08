using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PROYECT.Dominio.Enums;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
	public class TramiteDetalleValidator : AbstractValidator<TramiteDetalleDto>
	{
		private readonly IJurisdiccionRepositorio _jurisdiccionRepositorio;
		public TramiteDetalleValidator(IJurisdiccionRepositorio jurisdiccionRepositorio, long servicioId)
		{
			_jurisdiccionRepositorio = jurisdiccionRepositorio;
			ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

			RuleSet(RuleSetValidation.Default.ToString(), () =>
			{
				RuleFor(x => x.JurisdiccionId).Must(JurisdiccionEsInformanteValida)
					.WithMessage("La Jurisdicción Informante seleccionada no es válida o no está habilitada para informar");

				RuleFor(x => x.PlazoId).Must((dto, prop) => PlazoValidoParaJurisdiccionServicio(prop, dto.JurisdiccionId, servicioId))
					.WithMessage("El Plazo ingresado no es válido para la Jurisdicción Informante ingresada");
			});
		}

		private bool JurisdiccionEsInformanteValida(long jurisdiccionId)
		{
			var jurisdiccion = _jurisdiccionRepositorio.Get(jurisdiccionId);

			if (jurisdiccion == null) return false;

			return jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.Adherido ||
				   jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.Bloqueado;
		}

		private bool PlazoValidoParaJurisdiccionServicio(long plazoId, long jurisdiccionId, long servicioId)
		{
			var jurisdiccion = _jurisdiccionRepositorio.Get(jurisdiccionId);

			return jurisdiccion.Plazos.Any(jsp => jsp.VigenciaDesde <= DateTime.Now &&
										   (!jsp.VigenciaHasta.HasValue || jsp.VigenciaHasta.Value >= DateTime.Now) &&
										   jsp.PlazoId == plazoId && jsp.ServicioId == servicioId);
		}
	}
}
