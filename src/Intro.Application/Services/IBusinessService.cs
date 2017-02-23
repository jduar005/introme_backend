using System.Collections.Generic;

using Intro.Domain.PersistentModels;

namespace Intro.Application.Services
{
    public interface IBusinessService
    {
        IEnumerable<Business> GetAllBusinesses();

        Business Get(string id);

        // TODO change booleans to result objects
        IEnumerable<Business> AddBusiness(Business business);
        bool DeleteBusiness(Business business);
        Business UpdateBusiness(Business business);
    }
}