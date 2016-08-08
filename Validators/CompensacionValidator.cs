using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Helper.Repositorios.Contracts.Repositories;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions.Common;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.DTOs;

namespace PROYECT.WebAPI.Validators
{
    public class CompensacionValidator : AbstractValidator<CompensacionDto>
    {
        private readonly IRepositorio<Liquidacion> _liquidacionRepositorio;

        public CompensacionValidator(IRepositorio<Liquidacion> liquidacionRepositorio)
        {
            _liquidacionRepositorio = liquidacionRepositorio;

            RuleFor(x => x.EnteId)
                            .Must(ValidarTotal)
                            .WithMessage(Resources.Validacion_LiquidacionesSumarizadas);

            RuleFor(x => x.LiquidacionesIds)
                .Must(ValidarLiquidaciones)
                .WithMessage(Resources.Validacion_LiquidacionInvalida);

            RuleFor(x => x).Must(ExistenLiquidacionesPendientes)
                    .WithMessage(Resources.Validacion_LiquidacionInvalida_ExistenOtras);

            
            RuleFor(x => x.FechaDeposito)
                .NotEmpty().When(x => CalculaTotal(x.LiquidacionesIds,x.EnteId) != 0)
                .WithMessage(String.Format(Resources.Validacion_ValidDate, "Fecha Depósito"));

            RuleFor(x => x.FechaDeposito)
                .Must(x => x.ToDate() <= DateTime.Today)
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage(Resources.Validacion_Fecha);

            RuleFor(x => x.Entidad)
                .Length(10, 100)
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Entidad debe ser de mínimo 10 y máximo 100 caracteres.");

            RuleFor(x => x.Entidad)
                .Matches("^[0-9a-záéíóúüñA-ZÁÉÍÓÚÜÑ ]+$")
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Entidad solo permite caracteres alfanuméricos y espacios.");

            RuleFor(x => x.Sucursal)
                .Length(10, 100)
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Sucursal debe ser de mínimo 10 y máximo 100 caracteres.");

            RuleFor(x => x.Entidad)
                .Matches("^[0-9a-záéíóúüñA-ZÁÉÍÓÚÜÑ ]+$")
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Sucursal solo permite caracteres alfanuméricos y espacios.");

            RuleFor(x => x.Sucursal)
                .Length(10, 40)
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Cajero debe ser de mínimo 10 y máximo 40 caracteres.");

            RuleFor(x => x.Entidad)
                .Matches("^[a-záéíóúüñA-ZÁÉÍÓÚÜÑ ]+$")
                .When(x => CalculaTotal(x.LiquidacionesIds, x.EnteId) != 0)
                .WithMessage("El campo Cajero solo permite caracteres alfanuméricos.");

            RuleFor(x => x).NotEmpty();
         }

        private static bool ValidateFecha(String value)
        {
            var result = value.ToNuleableDate();
            return result != null;
        }
        
        private  bool ValidarLiquidaciones(List<long> liquidacionesId)
        {
            return liquidacionesId.Select(l => _liquidacionRepositorio.Get(l)).All(liquidacion => liquidacion != null);
        }

    
        private bool ValidarTotal(CompensacionDto compensacionDto, long jurisdiccionDestino)
        {
            return Total(compensacionDto, jurisdiccionDestino) <= 0;
           
        }
       
        private decimal Total(CompensacionDto compensacionDto, long jurisdiccionDestino)
        {
            decimal total = 0;
            foreach (var liquidacion in compensacionDto.LiquidacionesIds.Select(l => _liquidacionRepositorio.Get(l)))
            {
                if (liquidacion.OrigenId == jurisdiccionDestino)
                    total -= liquidacion.Importe;
                else
                    total += liquidacion.Importe;
            }
            return total;
        }

        private decimal CalculaTotal(List<long> liquidacionesIds, long jurisdiccionDestino)
        {
            decimal total = 0;
            foreach (var liquidacion in liquidacionesIds.Select(l => _liquidacionRepositorio.Get(l)))
            {
                if (liquidacion.OrigenId == jurisdiccionDestino)
                    total -= liquidacion.Importe;
                else
                    total += liquidacion.Importe;
            }
            return total;
        }
        private bool ExistenLiquidacionesPendientes(CompensacionDto compensacionDto)
        {
            var liquidacionesTotales = _liquidacionRepositorio.GetAll()
                .Where(x => (x.OrigenId == compensacionDto.JurisdiccionDestino && x.DestinoId == compensacionDto.EnteId) && x.Compensacion == null
                            && !compensacionDto.LiquidacionesIds.Contains(x.Id));
            return !liquidacionesTotales.Any();
        }
    }
}
