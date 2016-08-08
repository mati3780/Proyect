using System;
using System.Linq;
using System.Security.Claims;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;
using PROYECT.WebAPI.Extensions;

namespace PROYECT.WebAPI.Validators
{
    public class JurisdiccionValidator : AbstractValidator<JurisdiccionDto>
    {
        private readonly IRepositorio<Localidad> _localidadRepositorio;
        private readonly IRepositorio<Provincia> _provinciaRepositorio;
        private readonly IRepositorio<Jurisdiccion> _jurisdiccionRepositorio;

        public JurisdiccionValidator(IRepositorio<Localidad> localidadRepositorio, IRepositorio<Provincia> provinciaRepositorio, 
                                     IRepositorio<Jurisdiccion> jurisdiccionRepositorio)
        {
            _localidadRepositorio = localidadRepositorio;
            _jurisdiccionRepositorio = jurisdiccionRepositorio;
            _provinciaRepositorio = provinciaRepositorio;

            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Obtener.ToString(), () => {
                RuleFor(x => x.Id).Must(PuedeObtener).WithMessage(String.Format(Resources.Validacion_UsuarioNoPuedeObtener, "la jurisdicción"));
            });

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                
            });

            RuleSet(RuleSetValidation.Admin.ToString(), () => {
                RuleFor(x => x.Descripcion).Length(1, 250).Must(CheckDescripcionValida).WithMessage(Resources.Validacion_JurisdiccionDescripcionYaExiste)
                                                            .When(x => x.Descripcion != null).WithName("Denominación");

                RuleFor(x => x.TipoEstadoAdhesionId).GreaterThan(0);

                RuleFor(x => x.ProvinciaId).GreaterThan(0).Must(CheckExisteProvincia).WithMessage(String.Format(Resources.Validacion_NoExisteElementoA, "provincia"));

                RuleFor(x => x.GrupoAd).NotEmpty();
            });

            RuleSet(RuleSetValidation.Usuario.ToString(), () => {

            RuleFor(x => x.Email1).NotEmpty().Length(4, 100);

            RuleFor(x => x.Cuit);//.Matches("^[0-9]{2}-[0-9]{7,8}-[0-9]$");

            RuleFor(x => x.Sigla).Length(1, 6).When(x => x.Sigla != String.Empty);

            RuleFor(x => x.Autoridad).Length(1, 100).When(x => x.Autoridad != String.Empty);

            RuleFor(x => x.Direccion).Length(1, 250).When(x => x.Direccion != String.Empty);

            RuleFor(x => x.Email2).Length(1, 100).When(x => x.Email2 != String.Empty);

            RuleFor(x => x.Telefono).Length(1, 20).When(x => x.Telefono != String.Empty);

            RuleFor(x => x.Url).Length(1, 250).When(x => x.Url != String.Empty);

            RuleFor(x => x.CodigoPostal).Length(1, 10).When(x => x.CodigoPostal != String.Empty);

            RuleFor(x => x.Orden).LessThanOrEqualTo(Int32.MaxValue).When(x => x.Orden > 0);

            RuleFor(x => x.PlazoObservacion).LessThanOrEqualTo(Int16.MaxValue).When(x => x.PlazoObservacion > 0);

            RuleFor(x => x.PlazoRetiroDocumentacion).LessThanOrEqualTo(Int16.MaxValue).When(x => x.PlazoRetiroDocumentacion > 0);

            RuleFor(x => x.LocalidadId).GreaterThan(0).Must(CheckExisteLocalidad).WithMessage(String.Format(Resources.Validacion_NoExisteElementoA, "localidad"));

            RuleFor(x => x.LocalidadId).Must(CheckLocalidadProvincia).WithMessage(Resources.Validacion_LocalidadProvincia).When(x => x.LocalidadId > 0);

            });
            
            #region Example

            //RuleFor(x => x.MinNumberCharactersCitation)
            //       .NotNull()
            //       .WithMessage("Min. number of characters for citation is required")
            //       .GreaterThanOrEqualTo(1)
            //       .WithMessage("Min. number of characters for citation must be greater than or equal to 1")
            //       .LessThanOrEqualTo(x => x.MaxNumberCharactersCitation)
            //       .WithMessage("Min. number of characters must be less than or equal to max. number of characters");


            //RuleFor(x => x.Username)
            //.Must(x => x.Length >= 4)
            //.When(x => x.Username != null)
            //.WithState(x => new ErrorState
            //{
            //    ErrorCode = ErrorCode.TooShort,
            //    DeveloperMessageTemplate = "{0} must be at least 4 characters",
            //    DocumentationPath = "/Usernames",
            //    UserMessage = "Please enter a username with at least 4 characters"
            //});

            //RuleFor(x => x.Address.ZipCode)
            //    .Must(x => x != null)
            //    .When(x => x.Address != null)
            //    .WithState(x => new ErrorState
            //    {
            //        ErrorCode = ErrorCode.Required,
            //        DeveloperMessageTemplate = "{0} is required",
            //        DocumentationPath = "/Addresses",
            //        UserMessage = "Please enter a Zip Code"
            //    })

            //.WithLocalizedMessage(() => localizer.GetLocalized("ProductNotNullMessage"));

            #endregion
        }

        private bool PuedeObtener(Int64 id)
        {
            var result = _jurisdiccionRepositorio.Get(id);

            if (ClaimsPrincipal.Current.IsAdmin() || ClaimsPrincipal.Current.IsEnte())
                return true;

            return result.Id == ClaimsPrincipal.Current.GetJurisdiccionId();
        }

        private bool CheckLocalidadProvincia(JurisdiccionDto dto, Int64? localidadId)
        {
            if (!dto.LocalidadId.HasValue)
                return false;

	        var jurisdiccion = _jurisdiccionRepositorio.Get(dto.Id);
	        if (jurisdiccion == null)
		        return false;

			var localidad =  _localidadRepositorio.Get(dto.LocalidadId.Value);
            return localidad.Municipio.Provincia.Id == jurisdiccion.ProvinciaId;
        }

        private bool CheckExisteLocalidad(Int64? localidadId)
        {
            if (!localidadId.HasValue)
                return true;

            return _localidadRepositorio.GetAll().Any(x => x.Id == localidadId);
        }

        private bool CheckExisteProvincia(Int64 provinciaId)
        {
            return _provinciaRepositorio.GetAll().Any(x => x.Id == provinciaId);
        }

        private bool CheckDescripcionValida(JurisdiccionDto dto, string descripcion)
        {
            return !_jurisdiccionRepositorio.GetAll().Any(x => x.Descripcion == descripcion && x.ProvinciaId == dto.ProvinciaId && x.Id != dto.Id);
        }
    }
}