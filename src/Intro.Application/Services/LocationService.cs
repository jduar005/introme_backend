using System.Collections.Generic;
using System.Linq;

using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;

namespace Intro.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Business> _businessRepository;

        public LocationService(IRepository<Business> businessRepository)
        {
            this._businessRepository = businessRepository;
        }

        public IEnumerable<Location> GetAll() => this.Queryable;

        // TODO: Update to the new version of mongo and move the selectmany down to the database (rather than performing it after the ToList()
        public IEnumerable<Location> GetById(params string[] locationIds) => this.Queryable.Where(location => locationIds.Contains(location.Id));

        private IQueryable<Location> Queryable => this._businessRepository.GetAll().ToList().SelectMany(business => business.Locations).AsQueryable();
    }
}