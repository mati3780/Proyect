using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Http.ModelBinding;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.WebApi;
using PROYECT.Dominio.Enums;

namespace PROYECT.WebAPI.Extensions.Validation
{
    public static class ValidatorExtensions
    {
        public static bool IsValid<T>(this IValidator<T> validator, T instance, ModelStateDictionary modelState)
        {
            var result = validator.Validate(instance);

            if (result.IsValid)
                return true;

            result.AddToModelState(modelState, null);

            return false;
        }

        public static bool IsValid<T>(this IValidator<T> validator, T instance, ModelStateDictionary modelState, RuleSetValidation ruleSet)
        {
            var result = validator.Validate(instance, ruleSet: ruleSet.ToString());

            if (result.IsValid)
                return true;

            result.AddToModelState(modelState, null);

            return false;
        }

        public static bool IsValidWithDefault<T>(this IValidator<T> validator, T instance, ModelStateDictionary modelState, RuleSetValidation ruleSet, Boolean cascadeMode = false)
        {
            var result = validator.Validate(instance, ruleSet: $"{ruleSet},{RuleSetValidation.Default}");

            //if (result.IsValid || cascadeMode)
            //{
            //    foreach (var error in validator.Validate(instance, ruleSet: RuleSetValidation.Default.ToString()).Errors)
            //        result.Errors.Add(error);
            //}

            if (result.IsValid)
                return true;

            result.AddToModelState(modelState, null);

            return false;
        }
    }
}
