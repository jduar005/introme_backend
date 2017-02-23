using Intro.Application.Services;
using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;
using Nancy;
using Nancy.ModelBinding;

namespace Intro.Modules
{
    // TODO: test the customized content negotiation on these routes
    public class ItemModule : NancyModule
    {
        public ItemModule(IInventoryService inventoryService) : base("/item")
        {
            Get["/{id}"] = parameters => inventoryService.Get(parameters.Id);

            Get["/"] = parameters => new Inventory(inventoryService.GetAllItems());

            Post["/"] = parameters =>
            {
                //this.RequiresAuthentication();

                var item = this.Bind<Item>();
                var result = inventoryService.AddItem(item);

                //return this.RespondWith(result);
                return result;
            };

            Put["/"] = parameters =>
            {
                //this.RequiresAuthentication();

                var item = this.Bind<Item>();
                var result = inventoryService.UpdateItem(item);

                //return this.RespondWith(result);
                return result;
            };

            Delete["/"] = parameters =>
            {
                var item = this.Bind<Item>();
                return inventoryService.DeleteItem(item);
            };
        }
    }
}