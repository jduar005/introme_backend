using System;

using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Intro.Application.Extensions;
using Intro.Domain.ViewModels;

namespace Intro.Application.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IRepository<Business> _businessRepository;
        private readonly IRepository<PunchProgress> _progressRepository;

        public ProgressService(IRepository<Business> businessRepository, IRepository<PunchProgress> progressRepository)
        {
            this._businessRepository = businessRepository;
            this._progressRepository = progressRepository;
        }

        public PunchProgressSummary GetAllPunchProgress(string userName)
        {
            // TODO: get all for current user
            var allProgress =
                _progressRepository.GetAll()
                    .Where(IsUnredeemed(userName))
                    .ToList()
                    .HydrateWithBusinessName(this._businessRepository)
                    .ToList(); // just enumerate it into memory. there shouldn't be many punch progress for a user

            var reedemable = allProgress.Where(IsReedemable()).ToList();
            var recentlyIntro = allProgress.Where(
                IsRecentlyIntro())
                .OrderByDescending(p => p.PercentageComplete);
            var previouslyIntro = allProgress.Except(reedemable.Union(recentlyIntro));

            return new PunchProgressSummary
                       {
                           Reedemable = reedemable,
                           RecentlyIntro = recentlyIntro,
                           PreviouslyIntro = previouslyIntro
                       };
        }

        public IEnumerable<BusinessProgressSummary> GetAllBusinessProgress(string userName)
        {
            var unredeemedPunchProgress =
                _progressRepository.GetAll().Where(progress => progress.UserName == userName && !progress.DealRedeemed);
            var businessIds = unredeemedPunchProgress.Select(p => p.BusinessId).ToList();

            var businesses = _businessRepository.GetAll().Where(business => businessIds.Contains(business.Id));

            var summaries =
                businesses.Select(
                    business =>
                    new BusinessProgressSummary(business, unredeemedPunchProgress.Where(p => p.BusinessId == business.Id)));

            return summaries;
        }

        public BusinessProgressSummary GetProgressForSpecifiedBusiness(string userName, string businessId) // TODO: for current user
        {
            var unredeemedPunchProgress =
                _progressRepository.GetAll()
                    .Where(
                        progress =>
                        progress.UserName == userName && progress.BusinessId == businessId && !progress.DealRedeemed); // TODO: for current user

            var business = this._businessRepository.GetById(businessId); // TODO: validate that it exists
            if (business == null) throw new Exception($"No business found matching the id '{businessId}'"); // tODO: validation exception

            return new BusinessProgressSummary(business, unredeemedPunchProgress);
        }

        // TODO don't really need business id
        public PunchProgress RedeemDeal(string userName, string businessId, string dealId, Punch punch)
        {
            var business = _businessRepository.GetById(businessId);
            var deal = this.GetDealById(dealId);

            // TODO: encapsulate null assertion logic into helper method
            if (business == null) throw new Exception($"No business found matching the id '{businessId}'"); // tODO: validation exception
            if (deal == null) throw new Exception($"No deal found matching the id '{dealId}'"); // tODO: validation exception

            var progress = _progressRepository.LastOrDefault(PunchProgressMatchesRedeemedableDeal(userName, dealId));

            if (progress == null) throw new Exception($"No unredeedmed punch progress found matching deal id '{dealId}'"); // tODO: validation exception

            // punches aren't completed or the deal has already been redeemed 
            if (!progress.PunchesCompleted) throw new Exception("Punch progress has not yet been fully Intro!"); // tODO: validation exception
            if (progress.DealRedeemed) throw new Exception("Punch progress has already been redeemed!"); // todo: return validation message

            progress.DealRedeemed = true;
            progress.DealRedeemedTimeStamp = punch.TimeStamp;

            this._progressRepository.Update(progress);
            return progress;
        }

        public PunchProgress TrackPunch(string userName, string businessId, string dealId, Punch newPunch)
        {
            var business = _businessRepository.GetById(businessId);
            var deal = this.GetDealById(dealId);

            // TODO: encapsulate null assertion logic into helper method
            if (business == null) throw new Exception($"No business found matching the id '{businessId}'"); // tODO: validation exception
            if (deal == null) throw new Exception($"No deal found matching the id '{dealId}'"); // tODO: validation exception

            var existing = _progressRepository.Exists(PunchProgressMatchesUnredeemedableDeal(userName, dealId));

            var progress = existing
                           ? this.AddToExisting(userName, businessId, deal, newPunch)
                           : this.AddToNew(userName, businessId, deal, newPunch);

            return progress;
        }

        private Deal GetDealById(string dealId)
        {
            var allDeals = this._businessRepository.GetAll().ToList().SelectMany(business => business.Deals);
            var deal = allDeals.FirstOrDefault(d => d.Id == dealId);
            return deal;
        }

        private PunchProgress AddToNew(string userName, string businessId, Deal deal, Punch newPunch)
        {
            var progress = new PunchProgress(deal) { UserName = userName, BusinessId = businessId, DealId = deal.Id, Punches = new[] { newPunch } };

            this._progressRepository.Add(progress);
            return progress;
        }

        private PunchProgress AddToExisting(string userName, string businessId, Deal deal, Punch newPunch)
        {
            var progress = this._progressRepository.LastOrDefault(PunchProgressMatchesUnredeemedableDeal(userName, deal.Id));

            if (progress.PunchesCompleted)
            {
                // if punches for this progress have already been completed, create a new punch progress entity with the punch
                return this.AddToNew(userName, businessId, deal, newPunch);
            }; 

            progress.Punches = progress.Punches.Union(new[] { newPunch });

            this._progressRepository.Update(progress);
            return progress;
        }

        private static Func<PunchProgress, bool> IsUnredeemed(string userName)
            => progress => 
                    progress.UserName == userName && !progress.DealRedeemed;

        private static Func<PunchProgress, bool> IsReedemable()
            => progress => 
                    progress.PunchesCompleted && !progress.DealRedeemed;

        private static Func<PunchProgress, bool> IsRecentlyIntro()
            => progress =>
                    !progress.PunchesCompleted
                    && progress.Punches.Any(punch => punch.TimeStamp.AddDays(7) > DateTime.UtcNow);

        private static Expression<Func<PunchProgress, bool>> PunchProgressMatchesUnredeemedableDeal(string userName, string dealId)
            => progress =>
                    progress.UserName == userName && 
                    !progress.DealRedeemed &&
                    progress.DealId == dealId &&
                    !progress.PunchesCompleted;

        private static Expression<Func<PunchProgress, bool>> PunchProgressMatchesRedeemedableDeal(string userName, string dealId)
            => progress =>
                    progress.UserName == userName &&
                    progress.DealId == dealId &&
                    !progress.DealRedeemed;
    }
}