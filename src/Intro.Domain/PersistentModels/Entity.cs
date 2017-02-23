using System;

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

        public override bool Equals(object other)
        {
            var entity = other as Entity;
            return entity != null && this.Equals(entity);
        }

        public bool Equals(Entity other) => string.Equals(this.Id, other.Id);

        public override int GetHashCode() => this.Id?.GetHashCode() ?? 0;
    }
}