using System.Collections.Generic;

namespace Intro.Security
{
    // TODO G unit test
    public class UserIdentity : IUserIdentityAdaptor
    {
        public string UserName { get; private set; }

        public IEnumerable<string> Claims { get; private set; }

        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        public void SetClaims(IEnumerable<string> claims)
        {
            Claims = claims;
        }
    }
}