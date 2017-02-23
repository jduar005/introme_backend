using Intro.Application.Services;
using Intro.Domain.PersistentModels;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Intro.Modules
{
    public class ExchangeModule : NancyModule
    {
        public ExchangeModule(IInventoryService inventoryService) : base("/exchange")
        {
            Post["/pawn/{itemId}"] =
                parameters => inventoryService.AddNewExchange(parameters.itemId, this.Bind<Exchange>());

            Get["/consigment"] = parameters => new RedirectResponse("/index");
            
            Get["/trade"] = parameters => new RedirectResponse("/index");
        }
    }
}