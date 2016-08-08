using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
	public class PersonaValidator : AbstractValidator<PersonaDto>
	{
		readonly List<JurisdiccionServicioDato> _datosList;

		public PersonaValidator(long servicioId, long[] jurisdiccionesInformantesIds, IServicioRepositorio servicioRepositorio, bool validaConfiguracionRequerido)
		{
			var servicio = servicioRepositorio.GetConfiguracion(servicioId, jurisdiccionesInformantesIds, out _datosList);

			ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
			RuleSet(RuleSetValidation.Default.ToString(), () =>
				{
					RuleFor(x => x.Nombre).Length(1, 100).When(x => !string.IsNullOrEmpty(x.Nombre)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Nombre", 100));
					RuleFor(x => x.Apellido).Length(1, 100).When(x => !string.IsNullOrEmpty(x.Apellido)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Apellido", 100));
					RuleFor(x => x.ApellidoMaterno).Length(1, 100).When(x => !string.IsNullOrEmpty(x.ApellidoMaterno)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Apellido Materno", 100));
					RuleFor(x => x.NumeroDocumento).LessThanOrEqualTo(long.MaxValue).When(x => x.NumeroDocumento.HasValue);
					RuleFor(x => x.TipoDocumentoId).LessThanOrEqualTo(long.MaxValue).When(x => x.TipoDocumentoId.HasValue);
					RuleFor(x => x.Cuit).LessThanOrEqualTo(long.MaxValue).When(x => x.Cuit.HasValue);
					RuleFor(x => x.Cuit).Must(cuit => CuitHelper.CuitValido(cuit.Value)).When(x => x.Cuit.HasValue).WithMessage(Resources.Validacion_CuitCuilInvalido);
					RuleFor(x => x.RazonSocial).Length(1, 250).When(x => !string.IsNullOrEmpty(x.RazonSocial)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Denominación", 250));
					RuleFor(x => x.SociedadIGJId).Length(1, 20).When(x => !string.IsNullOrEmpty(x.SociedadIGJId)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Inscripción", 20));
					RuleFor(x => x.Domicilio).Length(1, 250).When(x => !string.IsNullOrEmpty(x.Domicilio)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Domicilio", 250));
					RuleFor(x => x.Tomo).Length(1, 4).When(x => !string.IsNullOrEmpty(x.Tomo)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Tomo", 4));
					RuleFor(x => x.Folio).Length(1, 4).When(x => !string.IsNullOrEmpty(x.Folio)).WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Folio", 4));

					if (validaConfiguracionRequerido)
					{
						RuleFor(x => x.Nombre)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("Nombre", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Nombre"));
						RuleFor(x => x.Apellido)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("Apellido", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Apellido"));
						RuleFor(x => x.ApellidoMaterno)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("ApellidoMaterno", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Apellido Materno"));
						RuleFor(x => x.NumeroDocumento)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("NumeroDocumento", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Número de Documento"));
						RuleFor(x => x.TipoDocumentoId)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("TipoDocumentoId", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Tipo Documento"));
						RuleFor(x => x.Cuit)
							.NotEmpty()
							.When(x => x.IsFisica && TipoDatoObligatorio("Cuil", TramiteDatoEntidad.PersonaFisica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "CUIT/CUIL"));

						RuleFor(x => x.RazonSocial)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("RazonSocial", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Denominación"));
						RuleFor(x => x.SociedadIGJId)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("Inscripcion", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Inscripción"));
						RuleFor(x => x.Domicilio)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("Domicilio", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Domicilio"));
						RuleFor(x => x.Tomo)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("TomoFolio", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Tomo"));
						RuleFor(x => x.Folio)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("TomoFolio", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "Folio"));
						RuleFor(x => x.Cuit)
							.NotEmpty()
							.When(x => !x.IsFisica && TipoDatoObligatorio("Cuit", TramiteDatoEntidad.PersonaJuridica))
							.WithMessage(string.Format(Resources.Validacion_Requerido, "CUIT"));
					}
				});
		}

		private bool TipoDatoObligatorio(string nombreDato, TramiteDatoEntidad tipoEntidad)
		{
			return _datosList.Any(jsd => jsd.TramiteDato.Nombre == nombreDato && jsd.TramiteDato.Entidad == tipoEntidad && jsd.Condicion == TramiteDatoCondicion.Requerido);
		}
	}
}
