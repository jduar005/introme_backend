using System;
using FluentValidation;
using Intro.Domain.PersistentModels;

namespace Intro.Application.Validation
{
    public class EntityValidator<T> : IEntityValidator<T> where T : Entity
    {
        private readonly Func<IEntityValidationResult> _resultFactory;
        private readonly Func<IEntityValidationResult<T>> _genericResultFactory;
        private readonly IValidatorFactory _validatorFactory;

        public EntityValidator(IValidatorFactory validatorFactory, Func<IEntityValidationResult> resultFactory, Func<IEntityValidationResult<T>> genericResultFactory)
        {
            this._validatorFactory = validatorFactory;
            this._resultFactory = resultFactory;
            this._genericResultFactory = genericResultFactory;
        }

        public IEntityValidationResult Validate(IEntity entity)
        {
            var result = this._resultFactory();
            result.Entity = entity;
            this.PopulateValidationResult(entity, result);

            return result;
        }

        public IEntityValidationResult<T> ValidateAsType(T entity)
        {
            var result = this._genericResultFactory();
            result.Entity = entity;
            this.PopulateValidationResult(entity, result);

            return result;
        }

        private void PopulateValidationResult<TEntity>(TEntity entity, IValidationResult result) where TEntity : IEntity
        {
            var validator = this._validatorFactory.GetValidator(entity.GetType());
            if (validator == null) return;

            var validationResult = validator.Validate(entity);
            result.AddErrors(validationResult.Errors);
        }
    }
}