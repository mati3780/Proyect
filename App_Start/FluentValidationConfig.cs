using System.Web.Http;
using FluentValidation.WebApi;
using SimpleInjector;
using PROYECT.WebAPI.Validators;

namespace PROYECT.WebAPI
{
    public static class FluentValidationConfig
    {
        public static void Configure(HttpConfiguration config, Container container)
        {
            FluentValidationModelValidatorProvider.Configure(config, x => { x.ValidatorFactory = new SimpleInjectorFluentValidatorFactory(container); });
        }
    }
}
