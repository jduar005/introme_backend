using System.Collections.Generic;
using Nancy.Security;

namespace Intro.Security
{
    public interface IUserIdentityAdaptor : IUserIdentity
    {
        void SetUserName(string userName);

        void SetClaims(IEnumerable<string> claims);
    }
}