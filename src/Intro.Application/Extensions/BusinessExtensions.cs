using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;

using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;

namespace Intro.Application.Extensions
{
    public static class BusinessExtensions
    {
        public static T HydrateWithCurrentDistance<T>(
            this T business,
            UserMetadata userMetadata,
            IRepository<GeoLocation> geoRepository)
            where T : Business
        {
            if (userMetadata == null || !userMetadata.HasLocation) return business;

            var geoNearHits = userMetadata.GetNearbyGeoHitsWhere(
                geoRepository,
                geoLoc => geoLoc.BusinessId == business.Id);


            business.HydrateDistances(geoNearHits.First(), geoNearHits);

            return business;
        }
            
        public static IEnumerable<T> SortUsingGeoLocations<T>(this IEnumerable<T> businesses, UserMetadata userMetadata, IRepository<GeoLocation> geoRepository)
            where T : Business
        {
            if (userMetadata == null || !userMetadata.HasLocation) return businesses;

            var currentBusinessIds = businesses.Select(biz => biz.Id);

            var nearbyGeoHits = userMetadata.GetNearbyGeoHitsWhere(
                geoRepository,
                geoLoc => currentBusinessIds.Contains(geoLoc.BusinessId));

            return businesses.ReorderByGeoLocationAndHydrateDistances(nearbyGeoHits);
        }

        private static GeoNearResult<GeoLocation>.GeoNearHits GetNearbyGeoHitsWhere(this UserMetadata userMetadata, IRepository<GeoLocation> geoRepository, Expression<Func<GeoLocation, bool>> locationFilter)
        {
            var geoNearOpts = new GeoNearArgs
                                  {
                                      // TODO: improve performance
                                      // only get the geo locations that apply to these businesses
                                      Query =
                                          Query<GeoLocation>.Where(locationFilter),
                                      Near =
                                          new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                                          userMetadata.CurrentLocation),
                                      IncludeLocs = true,
                                      Spherical = true,
                                      DistanceMultiplier = 0.00062137 // converting to miles
                                  };

            var geoNearHits = geoRepository.GeoNear(geoNearOpts).Hits;
            return geoNearHits;
        }

        private static IEnumerable<T> ReorderByGeoLocationAndHydrateDistances<T>(this IEnumerable<T> businesses, GeoNearResult<GeoLocation>.GeoNearHits allGeoHits)
            where T : Business
        {
            var reorderedAndHydrated = new List<T>();

            foreach (var geoHit in allGeoHits)
            {
                if (reorderedAndHydrated.Exists(biz => biz.Id == geoHit.Document.BusinessId)) continue; // skip it if we already processed it

                var business = businesses.FirstOrDefault(biz => biz.Id == geoHit.Document.BusinessId);

                business.HydrateDistances(geoHit, allGeoHits);

                reorderedAndHydrated.Add(business);
            }

            return reorderedAndHydrated;
        }

        private static void HydrateDistances<T>(this T business, GeoNearResult<GeoLocation>.GeoNearHit geoHit, GeoNearResult<GeoLocation>.GeoNearHits allGeoHits)
            where T : Business
        {
            if (geoHit == null || !allGeoHits.Any()) return;

            if (business.Locations.Count() == 1)
            {
                business.Locations.First().CurrentDistance = geoHit.Distance;
            }
            else
            {
                foreach (var location in business.Locations)
                {
                    var otherGeoLoc = allGeoHits.FirstOrDefault(loc => loc.Document.LocationId == location.Id);

                    if (otherGeoLoc != null)
                    {
                        location.CurrentDistance = otherGeoLoc.Distance;
                    }
                }

                business.Locations = business.Locations.OrderBy(loc => loc.CurrentDistance);
            }
        }
    }
}
