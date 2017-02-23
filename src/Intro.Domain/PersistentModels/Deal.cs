using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Intro.Domain.PersistentModels
{
    public class Deal : Entity
    {
        public Deal()
        {
        }

        public string DealName { get; set; }
        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public int NumberOfPunchesNeeded { get; set; }
    }
}