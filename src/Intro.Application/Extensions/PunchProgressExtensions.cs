using System.Collections.Generic;
using System.Linq;

using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;

namespace Intro.Application.Extensions
{
    public static class PunchProgressExtensions
    {
        public static void HydrateWithBusinessName(this PunchProgress punchProgress, IRepository<Business> businessRepository)
        {
            punchProgress.BusinessName = businessRepository.GetById(punchProgress.BusinessId)?.Name;
        }

        public static IEnumerable<PunchProgress> HydrateWithBusinessName(this IEnumerable<PunchProgress> punchProgress, IRepository<Business> businessRepository)
        {
            var businessIds = punchProgress.Select(p => p.BusinessId);
            var correspondingBusinesses = businessRepository.GetAll().Where(biz => businessIds.Contains(biz.Id));

            foreach (var progress in punchProgress)
            {
                var business = correspondingBusinesses.FirstOrDefault(biz => biz.Id == progress.BusinessId);
                var deal = business?.Deals.FirstOrDefault(d => d.Id == progress.DealId);

                progress.BusinessName = business?.Name;
                progress.DealName = deal?.DealName;
            }

            return punchProgress;
        }
    }
}