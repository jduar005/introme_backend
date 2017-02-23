
namespace Intro.Domain.PersistentModels
{
    public interface IAuthorizationId : IEntity
    {
        string Token { get; set; }
    }

    public class AuthorizationId : Entity, IAuthorizationId
    {
        public string Token { get; set; }
    }
}
