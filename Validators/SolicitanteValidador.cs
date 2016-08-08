using System;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
    public class SolicitanteValidador : AbstractValidator<SolicitanteDto>
    {
	    private readonly IRepositorio<Profesion> _profesionRepositorio;
	    private readonly IRepositorio<Localidad> _localidadRepositorio;

        public SolicitanteValidador(IRepositorio<Profesion> profesionRepositorio, IRepositorio<Localidad> localidadRepositorio)
        {
	        _profesionRepositorio = profesionRepositorio;
	        _localidadRepositorio = localidadRepositorio;
	        ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () =>
            {
                RuleFor(x => x.Nombre).NotEmpty().Length(1, 50);
                RuleFor(x => x.Apellido).NotEmpty().Length(1, 50);
                RuleFor(x => x.Domicilio).NotEmpty().Length(1, 250);
                RuleFor(x => x.CodigoPostal).NotEmpty().Length(1, 10);
				RuleFor(x => x.CodigoPostal).Matches("^(\\d{4}|[A-HJ-TP-Z]{1}\\d{4}[A-Z]{3}|[a-z]{1}\\d{4}[a-hj-tp-z]{3})$")
											.WithMessage("El campo Código Postal es inválido. Formatos permitidos: 9999 (CP antiguo) ó X9999YYY (Nuevo CPA)");
                RuleFor(x => x.Email1).NotEmpty().EmailAddress().Length(1, 100);
                RuleFor(x => x.Email2).Length(1, 100).EmailAddress().When(x => x.Email2 != String.Empty);
				RuleFor(x => x.TipoDocumentoId).NotEmpty().LessThanOrEqualTo(Int64.MaxValue);
				RuleFor(x => x.NumeroDocumento).NotEmpty();
                RuleFor(x => x.Telefono).Length(1, 20).When(x => x.Telefono != String.Empty);
                RuleFor(x => x.TelefonoMovil).Length(1, 20).When(x => x.TelefonoMovil != String.Empty);
                RuleFor(x => x.Cuit).NotEmpty();
	            RuleFor(x => x.Cuit).Must(x => CuitHelper.CuitValido(x)).When(x => x.Cuit != 0).WithMessage(Resources.Validacion_CuitCuilInvalido);

				RuleFor(x => x.ProfesionId).NotEmpty().LessThanOrEqualTo(Int64.MaxValue);
				RuleFor(x => x.Matricula).NotEmpty().When(MatriculaTomoFolioRequeridos).WithMessage("El campo Matrícula es requerido para la Profesión elegida");
	            RuleFor(x => x.Tomo).NotEmpty().When(MatriculaTomoFolioRequeridos).WithMessage("El campo Tomo es requerido para la Profesión elegida");
				RuleFor(x => x.Folio).NotEmpty().When(MatriculaTomoFolioRequeridos).WithMessage("El campo Folio es requerido para la Profesión elegida");

				RuleFor(x => x.Matricula).Empty().When(MatriculaTomoFolioMutuamenteExcluyentes).WithMessage("No puede especificar Matrícula y Tomo/Folio de manera simultánea. Si especifica Matrícula, no puede especificar Tomo/Folio y viceversa");
				RuleFor(x => x.Tomo).Empty().When(MatriculaTomoFolioMutuamenteExcluyentes).WithMessage("No puede especificar Matrícula y Tomo/Folio de manera simultánea. Si especifica Matrícula, no puede especificar Tomo/Folio y viceversa");
				RuleFor(x => x.Folio).Empty().When(MatriculaTomoFolioMutuamenteExcluyentes).WithMessage("No puede especificar Matrícula y Tomo/Folio de manera simultánea. Si especifica Matrícula, no puede especificar Tomo/Folio y viceversa");
				
				RuleFor(x => x.Matricula).Length(1, 40).When(x => x.Matricula != String.Empty);
                RuleFor(x => x.Tomo).Length(1, 4).When(x => x.Tomo != String.Empty);
                RuleFor(x => x.Folio).Length(1, 4).When(x => x.Folio != String.Empty);
                RuleFor(x => x.DatosMatriculacion).Length(1, 1000).When(x => x.DatosMatriculacion != String.Empty);
				RuleFor(x => x.ProvinciaId).NotEmpty().LessThanOrEqualTo(Int64.MaxValue);
				RuleFor(x => x.LocalidadId).NotEmpty().LessThanOrEqualTo(Int64.MaxValue);
	            RuleFor(x => x.LocalidadId).Must(LocalidadPerteneceAProvincia).When(x => x.LocalidadId != 0).WithMessage("La Localidad no coincide con la Provincia.");
            });
        }

	    private bool MatriculaTomoFolioRequeridos(SolicitanteDto solicitanteDto)
	    {
			return ProfesionRequiereMatriculaTomoFolio(solicitanteDto.ProfesionId) && 
					(string.IsNullOrWhiteSpace(solicitanteDto.Matricula) && 
					string.IsNullOrWhiteSpace(solicitanteDto.Tomo) && 
					string.IsNullOrWhiteSpace(solicitanteDto.Folio));
	    }

	    private bool MatriculaTomoFolioMutuamenteExcluyentes(SolicitanteDto solicitanteDto)
	    {
		    return !string.IsNullOrWhiteSpace(solicitanteDto.Matricula) &&
		            (!string.IsNullOrWhiteSpace(solicitanteDto.Tomo) || !string.IsNullOrWhiteSpace(solicitanteDto.Folio));
	    }

	    private bool ProfesionRequiereMatriculaTomoFolio(long profesionId)
	    {
		    var profesion = _profesionRepositorio.Get(profesionId);
		    return profesion != null && profesion.RequiereMatricula;
	    }

	    private bool LocalidadPerteneceAProvincia(SolicitanteDto solicitanteDto, long localidadId)
	    {
		    var localidad = _localidadRepositorio.Get(localidadId);
		    return localidad != null && localidad.Municipio.Provincia.Id == solicitanteDto.ProvinciaId;
	    }
    }
}