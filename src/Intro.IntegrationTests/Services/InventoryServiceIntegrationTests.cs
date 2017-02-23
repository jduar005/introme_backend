using FluentAssertions;
using Intro.Application.Services;
using Intro.DataAccess.Connection;
using Intro.Domain.PersistentModels;
using Intro.Utility.DotNetWrappers;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Intro.IntegrationTests.Services
{
    [TestFixture]
    public class InventoryServiceIntegrationTests
    {
        private IConnectionStringProvider connectionStringProvider;
        private MongoDbAccess _mongoDbAccess;
        private IInventoryService _service;
        private IClock _clock;

        [SetUp]
        public void BeforeEach()
        {
            connectionStringProvider = new MongoConnectionStringProvider();
            _mongoDbAccess = new MongoDbAccess(connectionStringProvider);
            _mongoDbAccess.DropAllCollections();

            _clock = Substitute.For<IClock>();
            _service = new InventoryService(new MongoRepository<Item>(connectionStringProvider), _clock);
        }

        private List<Item> GetItemsCollectionFromMongo()
        {
            return _mongoDbAccess.GetCollection<Item>().FindAll().ToList();
        }

        private WriteConcernResult InsertItemIntoMongoForTest(Item item)
        {
            return _mongoDbAccess.GetCollection<Item>().Insert(item);
        }

        [Test]
        public void AddItem_CreatesNewItemCorrectly()
        {
            var item = new Item
            {
                SerialNumber = "abcd12345"
            };

            _service.AddItem(item);
            var items = GetItemsCollectionFromMongo();

            items.Should().HaveCount(1);
            items.First().SerialNumber.Should().Be(item.SerialNumber);
        }

        [Test]
        public void AddItem_PersistsDateCreatedValue()
        {
            var time = new DateTime(1987, 11, 28).ToUniversalTime();
            _clock.UtcNow.Returns(time);
            var item = new Item();

            _service.AddItem(item);
            var items = GetItemsCollectionFromMongo();

            items.Should().HaveCount(1);
            items.First().DateCreated.Should().Be(time);
        }

        [Test]
        public void DeleteItem_RemovesItemCorrectly()
        {
            var itemOne = new Item
            {
                SerialNumber = "abcd12345"
            };
            var itemTwo = new Item
            {
                SerialNumber = "xyz789"
            };
            InsertItemIntoMongoForTest(itemOne);
            InsertItemIntoMongoForTest(itemTwo);

            _service.DeleteItem(itemOne);

            // assert that itemOne was deleted!
            var items = GetItemsCollectionFromMongo();
            items.Should().HaveCount(1);
            items.First().ShouldBeEquivalentTo(itemTwo);
        }

        [Test]
        public void UpdateItem_PersistsNewItemValuesCorrectly()
        {
            var itemOne = new Item
            {
                SerialNumber = "abcd12345"
            };
            var itemTwo = new Item
            {
                SerialNumber = "xyz789"
            };
            InsertItemIntoMongoForTest(itemOne);
            InsertItemIntoMongoForTest(itemTwo);

            // change itemTwo
            itemTwo.SerialNumber = "newSerialNumber";
            itemTwo.ItemCategories = new []{ "clothing" };
            _service.UpdateItem(itemTwo);

            // assert that itemTwo has the updated values in the DB!
            var items = GetItemsCollectionFromMongo();
            items.Should().HaveCount(2);
            var itemTwoResult = items.First(i => i.Id == itemTwo.Id);
            itemTwoResult.SerialNumber.Should().Be("newSerialNumber");
            itemTwoResult.ItemCategories.Should().HaveCount(1).And.Contain("clothing");
        }

    }
}
