using System;
using System.Globalization;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace PROYECT.WebAPI.Extensions.Common
{
    public static class StringExtensions
    {
        public static void AddToModelState(this String error, ModelStateDictionary modelState)
        {
            modelState.AddModelError("", error);
            modelState.SetModelValue("", new ValueProviderResult("", error, CultureInfo.CurrentCulture));
        }
    }
}
