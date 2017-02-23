
namespace Intro.Domain.ViewModels
{
    public class AuthenticationRequest
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }

    public class FacebookAuthenticationRequest
    {
        public string ShortLivedToken { get; set; }
    }
}
