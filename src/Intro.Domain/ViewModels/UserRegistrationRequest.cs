
namespace Intro.Domain.ViewModels
{
    public class UserRegistrationRequest
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class FacebookUserRegistrationRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string FbUserId { get; set; }

        public string FbImageUrl { get; set; }

        public string ShortLivedToken { get; set; }
    }
}
