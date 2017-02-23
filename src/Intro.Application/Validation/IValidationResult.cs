using System.Collections.Generic;
using FluentValidation.Results;

namespace Intro.Application.Validation
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        IDictionary<string, IEnumerable<string>> Errors { get; }

        void AddError(string name, string message);
        IValidationResult AddErrors(IEnumerable<ValidationFailure> validationFailures);
    }
}