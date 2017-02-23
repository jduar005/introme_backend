using System.Collections.Generic;
using System.Linq;
using Intro.Domain.PersistentModels;

namespace Intro.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(string userName);

        IQueryable<User> GetAllByIds(IEnumerable<string> userIds);
    }
}