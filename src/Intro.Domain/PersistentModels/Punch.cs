using System;
using MongoDB.Bson;

namespace Intro.Domain.PersistentModels
{
    public class Punch : Entity
    {
        public Punch()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public DateTime TimeStamp { get; set; }
        
        public string LocationId { get; set; }
    }
}