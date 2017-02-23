using System;
using System.Collections.Generic;
using System.Linq;

using Intro.Domain.PersistentModels;

namespace Intro.Domain.ViewModels
{
    public class PunchProgressSummary : IEntity
    {
        public string Id { get; set; }

        public IEnumerable<PunchProgress> Reedemable { get; set; }
        public IEnumerable<PunchProgress> RecentlyIntro { get; set; }
        public IEnumerable<PunchProgress> PreviouslyIntro { get; set; }
    }

    public class BusinessProgressSummary : Business
    {
        public BusinessProgressSummary(Business business, IEnumerable<PunchProgress> progress)
        {
            this.ThumbnailUrl = business.ThumbnailUrl;

            // inherited from Business
            Id = business.Id;
            BusinessType = business.BusinessType;
            DateJoined = business.DateJoined;
            Name = business.Name;
            Description = business.Description;
            Locations = business.Locations;

            // not inherited from business
            PunchProgress = progress;
            OtherDeals = GetDealsNotInProgress(business, PunchProgress);
        }

        private IEnumerable<Deal> GetDealsNotInProgress(Business business, IEnumerable<PunchProgress> progress)
        {
            var dealsInProgress = progress.Select(p => p.DealId);
            return business.Deals.Where(deal => !dealsInProgress.Contains(deal.Id));
        }

        public IEnumerable<PunchProgress> PunchProgress { get; set; }
        public IEnumerable<Deal> OtherDeals { get; set; }
    } 
}
