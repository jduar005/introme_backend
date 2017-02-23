using System;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Bson.Serialization.Attributes;

namespace Intro.Domain.PersistentModels
{
    public class PunchProgress : Deal
    {
        public PunchProgress()
        {
            Punches = new List<Punch>();
        }

        public PunchProgress(Deal deal)
        {
            DealName = deal.DealName;

            Description = deal.Description;
            NumberOfPunchesNeeded = deal.NumberOfPunchesNeeded;
            ThumbnailUrl = deal.ThumbnailUrl;

            // just in case
            DealRedeemed = false;
            DealRedeemedTimeStamp = null;
        }

        public string UserName { get; set; }

        public string DealId { get; set; }
        public string BusinessId { get; set; }

        public string BusinessName { get; set; }

        public bool DealRedeemed { get; set; }
        public DateTime? DealRedeemedTimeStamp { get; set; }

        public IEnumerable<Punch> Punches { get; set; }
        
        [BsonElement]
        public int NumberOfPunches => Punches.Count();

        [BsonElement]
        public bool PunchesCompleted => (Punches.Count() >= NumberOfPunchesNeeded);        

        [BsonIgnore]
        public double PercentageComplete => ((double)NumberOfPunches / (double)NumberOfPunchesNeeded)*((double)100);
    }
}