namespace Intro.Domain.PersistentModels
{
    public interface IUser : IEntity
    {
        string EmailAddress { get; set; }

        byte[] EncryptedPassword { get; set; }
    }

    public class User : Entity, IUser
    {
        public string EmailAddress { get; set; }

        public byte[] EncryptedPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class FacebookUser : User
    {
        public string FacebookId { get; set; }

        public string FacebookImageUrl { get; set; }
    }
}