using System.Collections.Generic;

namespace Intro.Security
{
    // TODO: need to change this to a claims principal somehow (https://github.com/NancyFx/Nancy/wiki/Nancy-v2-Upgrade-Notes)
    public interface IUserIdentity
    {
        /// <summary>The username of the authenticated user.</summary>
        /// <value>A <see cref="T:System.String" /> containing the username.</value>
        string UserName { get; }

        /// <summary>The claims of the authenticated user.</summary>
        /// <value>An <see cref="T:System.Collections.Generic.IEnumerable`1" />, containing the claims.</value>
        IEnumerable<string> Claims { get; }
    }
}