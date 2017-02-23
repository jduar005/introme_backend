using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Intro.Utility.Extensions;

namespace Intro.Application.Validation
{
    public class IntroValidationResult : IValidationResult
    {
        private readonly Dictionary<string, List<string>> _errors;

        public bool IsValid => !this._errors.Any();

        public IDictionary<string, IEnumerable<string>> Errors => this._errors.ToDictionary(error => error.Key, error => (IEnumerable<string>)error.Value);

        public IntroValidationResult()
        {
            this._errors = new Dictionary<string, List<string>>();
        }

        public IntroValidationResult(string name, string message)
            : this()
        {
            this.AddError(name, message);
        }

        public IEnumerable<IntroError> AsEnumerable()
        {
            return this._errors.Select(e => new IntroError
            {
                Name = e.Key,
                Messages = e.Value
            });    
        }

        public IValidationResult AddErrors(IEnumerable<ValidationFailure> validationFailures)
        {
            validationFailures.Each(error => this.AddError(error.PropertyName, error.ErrorMessage));

            return this;
        }

        public IValidationResult AddError(string name, string message)
        {
            if (!this._errors.ContainsKey(name))
            {
                this._errors.Add(name, new List<string> { message });
            }
            else
            {
                this._errors[name].Add(message);
            }

            return this;
        }
    }
}