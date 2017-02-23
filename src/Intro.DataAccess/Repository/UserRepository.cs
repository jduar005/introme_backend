using Intro.DataAccess.Connection;
using Intro.Domain.PersistentModels;
using System.Collections.Generic;
using System.Linq;

namespace Intro.DataAccess.Repository
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider)
        {
        }

        public User GetByUserName(string userName)
        {
            return this.GetAll().FirstOrDefault(u => u.EmailAddress == userName);
        }

        public IQueryable<User> GetAllByIds(IEnumerable<string> userIds)
        {
            return this.GetAll().Where(user => userIds.Contains(user.Id));
        }
    }
}
