using System;
using System.Collections.Generic;
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
    public class TramiteDatoListValidator : AbstractValidator<TramiteDatoListDto>
    {
        private readonly IRepositorio<Servicio> _servicioRepositorio;
        private readonly IRepositorio<TramiteDato> _tramiteDatosRepositorio;
        public TramiteDatoListValidator(IRepositorio<Servicio> servicioRepositorio, IRepositorio<TramiteDato> tramiteDatosRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
            _tramiteDatosRepositorio = tramiteDatosRepositorio;
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet(RuleSetValidation.Default.ToString(), () => {
                RuleFor(x => x).Must(CheckParametros).WithMessage(Resources.Validacion_TramiteDatoParametroFaltante);
                RuleFor(x => x).Must(CheckConditions).WithMessage(Resources.Validacion_TramiteDatoParametroCondicionFaltante);
                RuleFor(x => x).Must(CheckLabels).WithMessage(Resources.Validacion_TramiteDatoParametroLabelFaltante)
                               .Must(CheckLabelsLimites).WithMessage(Resources.Validacion_TramiteDatoParametroLabelExcede);
                RuleFor(x => x).Must(CheckNomenclaturaCatastral).WithMessage(Resources.Validacion_TramiteDatoNomenclatura);
                RuleFor(x=>x).Must(CheckMatriculaTomoLibro).WithMessage(Resources.Validacion_TramiteDatoMatriculaConflico);
            });

            RuleSet(RuleSetValidation.New.ToString(), () => {
                RuleFor(x => x).Must(Exists).WithMessage(Resources.Validacion_RegistroYaExiste);
            });

            RuleSet(RuleSetValidation.Edit.ToString(), () => { });
        }

        private bool CheckMatriculaTomoLibro(TramiteDatoListDto item)
        {
            if (!item.Inmueble)
                return true;
            
            var matriculaId = _tramiteDatosRepositorio.GetAll().Where(x => x.Identificador == TramiteDatoIdentificador.Matricula).Select(r => r.Id).Single();
            var tomoFolioId = _tramiteDatosRepositorio.GetAll().Where(x => x.Identificador == TramiteDatoIdentificador.TomoFolio).Select(r => r.Id).Single();

            var matriculaCondition = item.InmuebleDatos.Single(x => x.Value.Id == matriculaId).Value.Condicion;
            var tomoFolioCondition = item.InmuebleDatos.Single(x => x.Value.Id == tomoFolioId).Value.Condicion;

            if (matriculaCondition == TramiteDatoCondicion.Optativo || matriculaCondition == TramiteDatoCondicion.Deshabilitado)
            {
                if (tomoFolioCondition == TramiteDatoCondicion.Requerido)
                    return true;
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool CheckNomenclaturaCatastral(TramiteDatoListDto item)
        {
            if (!item.Inmueble)
                return true;

            var identificadores = new List<TramiteDatoIdentificador>
                                  {
                                      TramiteDatoIdentificador.Circunscripcion,
                                      TramiteDatoIdentificador.Manzana,
                                      TramiteDatoIdentificador.Parcela,
                                      TramiteDatoIdentificador.Seccion
                                  };

            var datos = _tramiteDatosRepositorio.GetAll().Where(x => identificadores.Contains((TramiteDatoIdentificador)x.Identificador)).Select(r => r.Id);
            var datosJurisdiccion = item.InmuebleDatos.Where(x => datos.Contains(x.Value.Id)).ToList();
            var ultimaCondicion = datosJurisdiccion.GroupBy(x => x.Value.Condicion);

            return ultimaCondicion.Count() == 1;
        }

        private bool CheckConditions(TramiteDatoListDto item)
        {
            var servicio = _servicioRepositorio.Get(item.ServicioId);
            var condiciones = new List<TramiteDatoCondicion> { TramiteDatoCondicion.Requerido, TramiteDatoCondicion.Optativo, TramiteDatoCondicion.Deshabilitado };
            
            if (servicio.Inmueble)
            {
                if (item.InmuebleDatos.Any(x => x.Value.Condicion == null || !condiciones.Contains((TramiteDatoCondicion)x.Value.Condicion)))
                    return false;
            }

            if (item.PersonaFisicaDatos.Any(x => x.Value.Condicion == null || !condiciones.Contains((TramiteDatoCondicion) x.Value.Condicion)))
                return false;

            return !item.PersonaJuridicaDatos.Any(x => x.Value.Condicion == null || !condiciones.Contains((TramiteDatoCondicion)x.Value.Condicion));
        }

        private bool CheckLabels(TramiteDatoListDto item)
        {
            var servicio = _servicioRepositorio.Get(item.ServicioId);

            if (servicio.Inmueble)
            {
                if (IsLabelNullValid(item.InmuebleDatos))
                    return false;
            }

            return true;
        }

        private bool CheckLabelsLimites(TramiteDatoListDto item)
        {
            var servicio = _servicioRepositorio.Get(item.ServicioId);

            if (servicio.Inmueble)
            {
                if (IsLabelLengthValid(item.InmuebleDatos))
                    return false;
            }

            return true;
        }

        private bool CheckParametros(TramiteDatoListDto item)
        {
            var servicio = _servicioRepositorio.Get(item.ServicioId);

	        return servicio.Datos
								.Where(dato => servicio.Inmueble)
								.All(dato => item.InmuebleDatos.Any(x => x.Value.Id == dato.TramiteDatoId));
        }

        private bool Exists(TramiteDatoListDto item)
        {
            var jurisdiccionId = ClaimsPrincipal.Current.GetJurisdiccionId();
            var servicio = _servicioRepositorio.Get(item.ServicioId);
            return servicio.Datos.All(x => x.Jurisdiccion.Id != jurisdiccionId);
        }

        private Boolean IsLabelLengthValid(IDictionary<String, TramiteDatoDto> dic)
        {
            return dic.Any(x => !String.IsNullOrEmpty(x.Value.Label) && x.Value.Label.Length > 100);
        }

        private Boolean IsLabelNullValid(IDictionary<String, TramiteDatoDto> dic)
        {
            return dic.Any(x => (x.Value.Condicion.HasValue && 
								(x.Value.Condicion.Value == TramiteDatoCondicion.Requerido || x.Value.Condicion.Value == TramiteDatoCondicion.Optativo)) && 
								String.IsNullOrEmpty(x.Value.Label));
        }
    }
}