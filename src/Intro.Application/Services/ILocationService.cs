using Intro.Domain.PersistentModels;
using Intro.Utility.DotNetWrappers;
using System.Collections.Generic;

using MongoDB.Bson;

using Intro.Domain.ViewModels;

namespace Intro.Application.Services
{
    public interface ILocationService
    {
        IEnumerable<Location> GetAll();
        IEnumerable<Location> GetById(params string[] locationIds);
    }
}
