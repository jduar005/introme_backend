
using MongoDB.Driver.GeoJsonObjectModel;

namespace Intro.Domain.ViewModels
{
    public class UserMetadata
    {
        public UserMetadata()
        {
        }

        public UserMetadata(double? longitude, double? latitude)
        {
            if (longitude == null || latitude == null) HasLocation = false;
            else
            {
                HasLocation = true;
                CurrentLocation = new GeoJson2DGeographicCoordinates(longitude.Value, latitude.Value);
            }
        }

        public bool HasLocation { get; set; }

        public GeoJson2DGeographicCoordinates CurrentLocation { get; set; }
    }
}
