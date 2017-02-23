using System;
using Autofac;
using FluentValidation;

namespace Intro.Application.Validation
{
    public class IntroValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _componentContext;

        public IntroValidatorFactory(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            object validator;
            this._componentContext.TryResolveKeyed(validatorType, typeof(IValidator), out validator);

            return (IValidator)validator;
        }
    }
}