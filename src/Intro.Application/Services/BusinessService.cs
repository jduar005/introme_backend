using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Utility.DotNetWrappers;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson;

using Intro.Domain.ViewModels;

namespace Intro.Application.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IRepository<Business> _businessRepository;
        private readonly IRepository<GeoLocation> _locationRepository;
        private readonly IClock _clock;

        public BusinessService(IRepository<Business> businessRepository, IRepository<GeoLocation> locationRepository, IClock clock)
        {
            this._businessRepository = businessRepository;
            this._locationRepository = locationRepository;

            _clock = clock;
        }

        public IEnumerable<Business> GetAllBusinesses()
        {
            return this._businessRepository.GetAll();
        }

        public Business Get(string id)
        {
            return this._businessRepository.GetById(id);
        }

        public IEnumerable<Business> AddBusiness(Business business)
        {
            // TODO validation!!
            //if (business.isValid())
            business.DateCreated = _clock.UtcNow;

            GenerateIdentificationForDealsAndLocations(business);

            this._businessRepository.Add(business);

            this.StoreGeoLocationsSeparetely(business);

            return this._businessRepository.GetAll();
        }

        private void StoreGeoLocationsSeparetely(Business business)
        {
            var geoLocations =
                business.Locations.Select(
                    loc => new GeoLocation { GeoPoint = loc.GeoPoint, BusinessId = business.Id, LocationId = loc.Id });

            this._locationRepository.Add(geoLocations);
        }

        // TODO: offer delete by Id?
        public bool DeleteBusiness(Business business)
        {
            //if (business.isValid())

            if (string.IsNullOrEmpty(business.Id)) return false;

            this._businessRepository.Delete(business);

            return true;
        }

        public Business UpdateBusiness(Business business)
        {
            // TODO validation!!
            //if (business.isValid())
            business.DateLastEdited = _clock.UtcNow;

            GenerateIdentificationForDealsAndLocations(business);

            return this._businessRepository.Update(business);
        }

        private static void GenerateIdentificationForDealsAndLocations(Business business)
        {
            foreach (var deal in business.Deals.Where(d => string.IsNullOrEmpty(d.Id)))
            {
                deal.Id = ObjectId.GenerateNewId().ToString();
            }

            foreach (var location in business.Locations.Where(loc => string.IsNullOrEmpty(loc.Id)))
            {
                location.Id = ObjectId.GenerateNewId().ToString();
            }
        }
    }
}
