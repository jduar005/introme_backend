using Intro.Domain.PersistentModels;
using Nancy.Security;

namespace Intro.Security
{
    public interface IUserIdentityAdaptorFactory
    {
        IUserIdentity FromUser(IUser user);
    }
}