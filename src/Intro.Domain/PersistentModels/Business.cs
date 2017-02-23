using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson.Serialization.Attributes;

namespace Intro.Domain.PersistentModels
{
    public class Business : Entity
    {
        public Business()
        {
            Locations = new List<Location>();
            Deals = new List<Deal>();
        }

        public string BusinessType { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateLastEdited { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? DateJoined { get; set; }

        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<Deal> Deals { get; set; }

        public string ThumbnailUrl { get; set; }

        [BsonIgnore]
        public double? DistanceToClosestLocation
            =>
                Locations.Any(loc => loc.CurrentDistance != null)
                    ? Locations.Where(loc => loc.CurrentDistance != null).Min(loc => loc.CurrentDistance)
                    : null;
    }
}