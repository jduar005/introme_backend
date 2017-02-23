using System;
using Intro.Domain.PersistentModels;
using Nancy.Security;

namespace Intro.Security
{
    // TODO G unit test
    public class UserIdentityAdaptorFactory : IUserIdentityAdaptorFactory
    {
        private readonly Func<IUserIdentityAdaptor> _userIdentityFactory;

        public UserIdentityAdaptorFactory(Func<IUserIdentityAdaptor> userIdentityFactory)
        {
            this._userIdentityFactory = userIdentityFactory;
        }

        public IUserIdentity FromUser(IUser user)
        {
            var userIdentity = _userIdentityFactory();
            userIdentity.SetUserName(user.UserName);
            userIdentity.SetClaims(new[] { user.Id });

            return userIdentity;
        }
    }
}