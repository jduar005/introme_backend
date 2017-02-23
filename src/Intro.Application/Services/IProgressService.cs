using System.Collections.Generic;

using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;

namespace Intro.Application.Services
{
    public interface IProgressService
    {
        PunchProgressSummary GetAllPunchProgress(string userName);
        IEnumerable<BusinessProgressSummary> GetAllBusinessProgress(string userName);
        BusinessProgressSummary GetProgressForSpecifiedBusiness(string userName, string businessId);

        PunchProgress TrackPunch(string userName, string businessId, string dealId, Punch newPunch);
        PunchProgress RedeemDeal(string userName, string businessId, string dealId, Punch punch);
    }
}