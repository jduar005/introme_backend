using Intro.Domain.PersistentModels;

namespace Intro.Application.Validation
{
    public interface IEntityValidator<T> where T : Entity
    {
        IEntityValidationResult Validate(IEntity entity);

        IEntityValidationResult<T> ValidateAsType(T entity);
    }
}