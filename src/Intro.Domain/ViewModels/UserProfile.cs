using Intro.Domain.PersistentModels;

namespace Intro.Domain.ViewModels
{
    // TODO: create an interface without an id
    public class UserProfile : IEntity
    {
        public UserProfile()
        {
        }

        public IUser Identity { get; set; }

        public AuthorizationId Auth { get; set; }

        // TODO: maybe keep track of last reported location?
//        public GeoJson2DGeographicCoordinates LastReportedLocation { get; set; }
        
        // TODO: get rid of it
        public string Id { get; set; }
    }
}
