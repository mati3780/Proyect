using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
    public class SolicitudValidator : AbstractValidator<SolicitudDto>
    {
        private readonly IJurisdiccionRepositorio _jurisdiccionRepositorio;
        private readonly IServicioRepositorio _servicioRepositorio;
        private readonly ITasaNacionalRepositorio _tasaNacionalRepositorio;

        public SolicitudValidator(IJurisdiccionRepositorio jurisdiccionRepositorio, IServicioRepositorio servicioRepositorio,
                                  IValidator<SolicitanteDto> solicitanteValidator, ITasaNacionalRepositorio tasaNacionalRepositorio)
        {
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _tasaNacionalRepositorio = tasaNacionalRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () =>
            {
                RuleFor(x => x.Solicitante).NotEmpty();
                RuleFor(x => x.Solicitante).SetValidator(solicitanteValidator);
                RuleFor(x => x.JurisdiccionRequirenteId)
                            .NotEmpty()
                            .LessThanOrEqualTo(long.MaxValue);
                RuleFor(x => x.JurisdiccionRequirenteId)
                            .Must(JurisdiccionEsRequirenteValida)
                            .WithMessage("La Jurisdicción Requirente seleccionada no es válida o no está habilitada para actuar como requirente");
                RuleFor(x => x.ReparticionSolicitanteId)
                            .NotEmpty()
                            .When(s => JurisdiccionEsReparticionSolicitante(s.JurisdiccionRequirenteId))
                            .WithMessage("El campo Repartición Solicitante es obligatorio si la Jurisdicción Requirente es una repartición ");
                RuleFor(x => x.MotivoConsulta)
                            .NotEmpty()
                            .WithMessage(string.Format(Resources.Validacion_Requerido, "Motivo de consulta"));
                RuleFor(x => x.MotivoConsulta)
                            .Length(10, 1000)
                            .WithMessage("El campo Motivo de Consulta debe ser de mínimo 10 y máximo 1000 caracteres");
                RuleFor(x => x.MotivoConsulta)
                            .Matches("^[0-9a-záéíóúüñA-ZÁÉÍÓÚÜÑ ]+$")
                            .WithMessage("El campo Motivo de Consulta solo permite caracteres alfanuméricos y espacios");
                RuleFor(x => x.ServicioId)
                            .NotEmpty()
                            .WithMessage(string.Format(Resources.Validacion_Requerido, "Servicio"));
                RuleFor(x => x.Observacion)
                            .Length(1, 1000)
                            .When(x => !string.IsNullOrWhiteSpace(x.Observacion))
                            .WithMessage(string.Format(Resources.Validacion_TamanoMaximo, "Observación", 1000));
                RuleFor(x => x.Entidad).NotNull();
                RuleFor(x => x.Entidad.Persona)
                            .SetValidator(x => new PersonaValidator(x.ServicioId, x.Tramites.Select(td => td.JurisdiccionId).ToArray(), _servicioRepositorio, true))
                            .When(x => !ServicioEsInmueble(x.ServicioId));
                RuleFor(x => x.Entidad.Inmueble)
                            .SetValidator(x => new InmuebleValidator(x.ServicioId, x.Tramites.Select(td => td.JurisdiccionId).ToArray(), _servicioRepositorio))
                            .When(x => ServicioEsInmueble(x.ServicioId));
                RuleFor(x => x.Tramites)
                            .NotEmpty();
                RuleFor(x => x.Tramites)
                            .Must(x => x.Any())
                            .WithMessage("Debe ingresar al menos una Jurisdicción Informante");
                RuleFor(x => x.Tramites)
                            .Must((dto, x) => x.Count == 1)
                            .When(x => ServicioEsInmueble(x.ServicioId))
                            .WithMessage("El servicio seleccionado permite solamente una (1) Jurisdicción Informante");
                RuleFor(x => x.Tramites)
                            .Must((dto, x) => x.Select(t => t.JurisdiccionId).Distinct().Count() == x.Select(t => t.JurisdiccionId).Count())
                            .When(x => x.Tramites.Any())
                            .WithMessage("Se detectaron Jurisdicciones Informantes duplicadas");
                RuleFor(x => x.Tramites)
                            .Must((dto, x) => x.All(t => t.JurisdiccionId != dto.JurisdiccionRequirenteId))
                            .When(x => x.Tramites.Any())
                            .WithMessage("No se puede ingresar como Jurisdicción Informante la misma Jurisdicción que se encuentra seleccionada como Requirente");
                RuleFor(x => x.Tramites)
                            .SetCollectionValidator(x => new TramiteDetalleValidator(jurisdiccionRepositorio, x.ServicioId));

            });

            RuleSet(RuleSetValidation.Edit.ToString(), () =>
            {
                RuleFor(x => x.TasaNacionalId)
                            .Must(TasaNacionalValida)
                            .WithMessage("El Formulario PROYECT del trámite no se encuentra vigente");
            });
        }

        private bool JurisdiccionEsRequirenteValida(long jurisdiccionId)
        {
            var jurisdiccion = _jurisdiccionRepositorio.Get(jurisdiccionId);

            if (jurisdiccion == null) return false;

            return jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.Adherido ||
                   jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ReparticionRequirente ||
                   jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.BloqueadoInformante;
        }

        private bool JurisdiccionEsReparticionSolicitante(long jurisdiccionId)
        {
            var jurisdiccion = _jurisdiccionRepositorio.Get(jurisdiccionId);
            return jurisdiccion.TipoEstadoAdhesion.Identificador == TipoEstadoAdhesionIdentificador.ReparticionRequirente;
        }

        private bool ServicioEsInmueble(long servicioId)
        {
            var servicio = _servicioRepositorio.Get(servicioId);
            return servicio.Inmueble;
        }

        private bool TasaNacionalValida(long tasaNacionalId)
        {
            return _tasaNacionalRepositorio.GetVigenteId() == tasaNacionalId;
        }
    }
}