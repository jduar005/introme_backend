using System.Collections.Generic;
using Nancy.Security;

namespace Intro.Security
{
//    public interface IUserIdentityAdaptor : IUserIdentity
    public interface IUserIdentityAdaptor
    {
        void SetUserName(string userName);

        void SetClaims(IEnumerable<string> claims);
    }
}