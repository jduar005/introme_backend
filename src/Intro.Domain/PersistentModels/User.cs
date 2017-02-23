namespace Intro.Domain.PersistentModels
{
    public interface IUser : IEntity
    {
        string UserName { get; set; }

        byte[] EncryptedPassword { get; set; }
    }

    public class User : Entity, IUser
    {
        public string UserName { get; set; }

        public byte[] EncryptedPassword { get; set; }
    }
}