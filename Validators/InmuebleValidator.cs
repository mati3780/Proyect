using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PROYECT.Dominio.Contracts;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
	public class InmuebleValidator : AbstractValidator<InmuebleDto>
	{
		readonly List<JurisdiccionServicioDato> _datosList;

		public InmuebleValidator(long servicioId, long[] jurisdiccionesInformantesIds, IServicioRepositorio servicioRepositorio)
		{
			var servicio = servicioRepositorio.GetConfiguracion(servicioId, jurisdiccionesInformantesIds, out _datosList);

			ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

			RuleSet(RuleSetValidation.Default.ToString(), () =>
			{
				RuleFor(x => x.TomosFolios).SetCollectionValidator(new TomoFolioValidator());
				RuleFor(x => x.PersonasFisicas).SetCollectionValidator(new PersonaValidator(servicioId, jurisdiccionesInformantesIds, servicioRepositorio, false));
				RuleFor(x => x.PersonasJuridicas).SetCollectionValidator(new PersonaValidator(servicioId, jurisdiccionesInformantesIds, servicioRepositorio, false));

				RuleFor(x => x.UbicacionInmueble)
						.Length(1, 100)
						.When(x => !string.IsNullOrEmpty(x.UbicacionInmueble))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Ubicación Inmueble", 100));

				RuleFor(x => x.Matricula)
						.NotEmpty()
						.When(x => !x.TomosFolios.Any() && TipoDatoObligatorio("Matricula", TramiteDatoEntidad.Inmueble))
						.WithMessage(string.Format(Resources.Validacion_Requerido, "Matrícula"));
				RuleFor(x => x.TomosFolios)
						.Must(x => x.Any())
						.When(x => string.IsNullOrEmpty(x.Matricula) && TipoDatoObligatorio("TomoFolio", TramiteDatoEntidad.Inmueble))
						.WithMessage("Debe ingresar al menos un Tomo/Folio");

				RuleFor(x => x.Matricula)
						.Empty()
						.When(x => x.TomosFolios.Any())
						.WithMessage("No puede especificar simultáneamente Matrícula y Tomo/Folio. Si especifica Matrícula, no puede especificar Tomo y/o Folio y viceversa.");
				RuleFor(x => x.TomosFolios)
						.Must(x => !x.Any())
						.When(x => !string.IsNullOrEmpty(x.Matricula))
						.WithMessage("No puede especificar simultáneamente Tomo/Folio y Matrícula. Si especifica Matrícula, no puede especificar Tomo y/o Folio y viceversa.");

				RuleFor(x => x.Zona)
						.Length(1, 9)
						.When(x => !string.IsNullOrEmpty(x.Zona))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Zona", 9));
				RuleFor(x => x.UnidadComplementaria)
						.Length(1, 20)
						.When(x => !string.IsNullOrEmpty(x.UnidadComplementaria))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Unidad Complementaria", 20));
				RuleFor(x => x.Lote)
						.Length(1, 20)
						.When(x => !string.IsNullOrEmpty(x.Lote))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Lote", 20));
				RuleFor(x => x.Manzana)
						.Length(1, 20)
						.When(x => !string.IsNullOrEmpty(x.Manzana))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Manzana", 20));
				RuleFor(x => x.Legajo)
						.Length(1, 20)
						.When(x => !string.IsNullOrEmpty(x.Legajo))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Legajo", 20));
				RuleFor(x => x.NomenclaturaCatastralCircunscripcion)
						.Length(1, 3)
						.When(x => !string.IsNullOrEmpty(x.NomenclaturaCatastralCircunscripcion))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Circunscripción", 3));
				RuleFor(x => x.NomenclaturaCatastralSeccion)
						.Length(1, 3)
						.When(x => !string.IsNullOrEmpty(x.NomenclaturaCatastralSeccion))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Sección", 3));
				RuleFor(x => x.NomenclaturaCatastralManzana)
						.Length(1, 3)
						.When(x => !string.IsNullOrEmpty(x.NomenclaturaCatastralManzana))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Manzana", 3));
				RuleFor(x => x.NomenclaturaCatastralParcela)
						.Length(1, 6)
						.When(x => !string.IsNullOrEmpty(x.NomenclaturaCatastralParcela))
						.WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Parcela", 6));
				RuleFor(x => x.Superficie)
						.LessThanOrEqualTo(long.MaxValue)
						.When(x => x.Superficie.HasValue);
				RuleFor(x => x.UnidadMedidaId)
						.LessThanOrEqualTo(long.MaxValue)
						.When(x => x.UnidadMedidaId.HasValue);

				RuleFor(x => x.UbicacionInmueble)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("UbicacionInmueble", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Ubicación Inmueble"));
				RuleFor(x => x.Zona)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("Zona", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Zona"));
				RuleFor(x => x.Lote)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("Lote", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Lote"));
				RuleFor(x => x.Manzana)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("Manzana", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Manzana"));
				RuleFor(x => x.Legajo)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("Legajo", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Legajo"));
				RuleFor(x => x.Superficie)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("Superficie", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Superficie"));
				RuleFor(x => x.UnidadMedidaId)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("UnidadMedida", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Unidad de Medida"));
				RuleFor(x => x.UnidadComplementaria)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("UnidadFuncional", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Unidad Funcional"));
				RuleFor(x => x.NomenclaturaCatastralCircunscripcion)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("NomenclaturaCatastralCircunscripcion", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Circunscripción"));
				RuleFor(x => x.NomenclaturaCatastralSeccion)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("NomenclaturaCatastralSeccion", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Sección"));
				RuleFor(x => x.NomenclaturaCatastralManzana)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("NomenclaturaCatastralManzana", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Manzana"));
				RuleFor(x => x.NomenclaturaCatastralParcela)
							.NotEmpty()
							.When(x => TipoDatoObligatorio("NomenclaturaCatastralParcela", TramiteDatoEntidad.Inmueble))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Parcela"));
			});
		}

		private bool TipoDatoObligatorio(string nombreDato, TramiteDatoEntidad tipoEntidad)
		{
			return _datosList.Any(jsd => jsd.TramiteDato.Nombre == nombreDato && jsd.TramiteDato.Entidad == tipoEntidad && jsd.Condicion == TramiteDatoCondicion.Requerido);
		}
	}
}
