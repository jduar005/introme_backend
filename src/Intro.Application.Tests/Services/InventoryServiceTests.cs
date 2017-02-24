using Intro.Application.Services;
using Intro.DataAccess.Repository;
using Intro.Domain.PersistentModels;
using Intro.Domain.PersistentModels.Old;
using Intro.Utility.DotNetWrappers;
using NSubstitute;
using NUnit.Framework;

namespace Intro.Application.Tests.Services
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private IClock _clock;
        private IRepository<Item> _repository;
        private IInventoryService _service;

        [SetUp]
        public void BeforeEach()
        {
            _clock = Substitute.For<IClock>();
            _repository = Substitute.For<IRepository<Item>>();
            _service = new InventoryService(_repository, _clock);
        }

        [Test]
        public void GetAllItems_InvokesRepository()
        {
            _service.GetAllItems();

            _repository.Received(1).GetAll();
        }

        [Test]
        public void Get_InvokesRepository_WithId()
        {
            string id = "11235";
            
            _service.Get(id);

            _repository.Received(1).GetById(id);
        }

        [Test]
        public void AddItem()
        {
        }

    }
}
