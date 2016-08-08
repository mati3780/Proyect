using System;
using FluentValidation;

namespace PROYECT.WebAPI.Validators
{
    public class SimpleInjectorFluentValidatorFactory : ValidatorFactoryBase
    {
        private IServiceProvider Injector { get; set; }

        public SimpleInjectorFluentValidatorFactory(IServiceProvider injector)
        {
            Injector = injector;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return Injector.GetService(validatorType) as IValidator;
        }
    }
}