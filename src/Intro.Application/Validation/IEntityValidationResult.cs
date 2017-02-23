using Intro.Domain.PersistentModels;

namespace Intro.Application.Validation
{
    public interface IEntityValidationResult : IValidationResult
    {
        IEntity Entity { get; set; }
    }

    public interface IEntityValidationResult<T> : IValidationResult where T : Entity
    {
        T Entity { get; set; }
    }
}