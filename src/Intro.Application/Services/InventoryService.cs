using System.Linq;
using Intro.DataAccess;
using Intro.DataAccess.Repository;
using Intro.Domain;
using Intro.Domain.PersistentModels;
using Intro.Domain.ViewModels;
using Intro.Utility.DotNetWrappers;
using System.Collections.Generic;

using Intro.Domain.PersistentModels.Old;

namespace Intro.Application.Services
{
    // TODO: config mongo db so that you don't always get all exchanges with item
    public class InventoryService : IInventoryService
    {
        private readonly IRepository<Item> _repository;
        private readonly IClock _clock;

        public InventoryService(IRepository<Item> repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _repository.GetAll();
        }

        public Item Get(string id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Item> AddItem(Item Item)
        {
            //if (Item.isValid())
            var now = _clock.UtcNow;
            Item.DateCreated = now;
            _repository.Add(Item);

            return _repository.GetAll();
        }
        
        // TODO: offer delete by Id?
        public bool DeleteItem(Item Item)
        {
            //if (Item.isValid())

            if (string.IsNullOrEmpty(Item.Id)) return false;

            _repository.Delete(Item);

            return true;
        }

        public Item UpdateItem(Item item)
        {
            // TODO validation!!
            //if (Item.isValid())
            item.DateLastEdited = _clock.UtcNow;

            return _repository.Update(item);
        }

        public bool AddNewExchange(string itemId, Exchange exchange)
        {
            if (!_repository.Exists(entity => entity.Id == itemId))
            {
                // TODO be more descriptive with errors... don't just return bool
                return false; // i.e., return "not found" message
            }

            var item = _repository.GetById(itemId);

            exchange.DateCreated = _clock.UtcNow;
            item.Exchanges.Add(exchange);

            _repository.Update(item);
            return true;
        }

        // TODO: figure out a way to update a sub-entity without getting the parent Entity (item) first
        public bool FinalizeExchange(string itemId, Exchange exchange)
        {
            if (!_repository.Exists(entity => entity.Id == itemId))
            {
                // TODO be more descriptive with errors... don't just return bool
                return false; // i.e., return "not found" message
            }

            var item = _repository.GetById(itemId);
            var exchangeToFinalize = item.Exchanges.FirstOrDefault(ex => ex.Id == exchange.Id);

            if (exchangeToFinalize == default(Exchange))
            {
                // TODO be more descriptive with errors... don't just return bool
                return false;
            }

            exchangeToFinalize.DateFinalized = _clock.UtcNow;

            // TODO write integration test to check if this actually updated the exchange with the date finalized
            _repository.Update(item);
            return true;
        }
    }
}
