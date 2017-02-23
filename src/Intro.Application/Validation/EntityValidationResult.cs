using Intro.Domain.PersistentModels;

namespace Intro.Application.Validation
{
    public class EntityValidationResult : IntroValidationResult, IEntityValidationResult
    {
        public IEntity Entity { get; set; }
    }

    public class EntityValidationResult<T> : IntroValidationResult, IEntityValidationResult<T>
        where T : Entity
    {
        public T Entity { get; set; }
    }
}