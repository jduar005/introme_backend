using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Intro.Domain.PersistentModels
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}