using System.Collections.Generic;
using Intro.Domain.PersistentModels;
using Intro.Domain.PersistentModels.Old;

namespace Intro.Application.Services
{
    public interface IInventoryService
    {
        IEnumerable<Item> GetAllItems();

        Item Get(string id);

        // TODO change booleans to 
        IEnumerable<Item> AddItem(Item Item);
        bool DeleteItem(Item Item);
        Item UpdateItem(Item item);

        bool AddNewExchange(string itemId, Exchange exchange);
        bool FinalizeExchange(string itemId, Exchange exchange);
    }
}