using System;
using System.Linq;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Intro.Domain.PersistentModels
{
    public class GeoLocation : Entity
    {
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> GeoPoint { get; set; }
        public string BusinessId { get; set; }
        public string LocationId { get; set; }
    }

    public class Location : Entity
    {
        public Location()
        {
            Country = "United States of America";
        }

        public DateTime DateEstablished { get; set; }
        
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> GeoPoint { get; set; }
        public string StreetOne { get; set; }
        public string StreetTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; } // TODO: validation (Regex)

        [BsonIgnore]
        public double? CurrentDistance { get; set; }
    }
}