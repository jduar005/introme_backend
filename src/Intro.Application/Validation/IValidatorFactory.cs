using System;
using FluentValidation.Results;
using Intro.Domain.PersistentModels;

namespace Intro.Application.Validation
{
    public interface IValidator
    {
        ValidationResult Validate(IEntity entity);
    }

    public interface IValidatorFactory
    {
        /// <summary>Gets the validator for the specified type.</summary>
        IValidator GetValidator(Type type);
    }
}