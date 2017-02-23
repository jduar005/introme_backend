using FluentAssertions;
using Intro.DataAccess;
using Intro.DataAccess.Connection;
using Intro.IntegrationTests.Domain;
using NUnit.Framework;

namespace Intro.IntegrationTests.DataAccess
{
    [TestFixture]
    public class MongoCollectionManagerTests
    {
        private IConnectionStringProvider connectionStringProvider;
        private MongoDbAccess _mongoDbAccess;

        [SetUp]
        public void BeforeEach()
        {
            connectionStringProvider = new MongoConnectionStringProvider();

            _mongoDbAccess = new MongoDbAccess(connectionStringProvider);

            _mongoDbAccess.DropAllCollections();
        }

        [Test]
        public void GetDatabase_GetsDatabase()
        {
            _mongoDbAccess.GetDatabase().Should().NotBeNull();
        }

        [Test]
        public void GetCollection_GetsCollection()
        {
            _mongoDbAccess.GetCollection<TestEntity>().Should().NotBeNull();
        }

        [Test]
        public void CollectionExists_WhenCollectionExists_ReturnsTrue()
        {
            _mongoDbAccess.GetCollection<TestEntity>().Insert(new TestEntity());

            _mongoDbAccess.CollectionExists<TestEntity>().Should().BeTrue();
        }

        [Test]
        public void CollectionExists_WhenCollectionDoesNotExist_ReturnsFalse()
        {
            _mongoDbAccess.CollectionExists<TestEntity>().Should().BeFalse();
        }

        [Test]
        public void DropAllCollections_DropsAllCollections()
        {
            _mongoDbAccess.GetCollection<TestEntity>().Insert(new TestEntity());
            _mongoDbAccess.GetCollection<TestObject>().Insert(new TestObject());

            _mongoDbAccess.DropAllCollections();

            _mongoDbAccess.CollectionExists<TestEntity>().Should().BeFalse();
            _mongoDbAccess.CollectionExists<TestObject>().Should().BeFalse();
        }

        [Test]
        public void DropCollection_DropsCollection()
        {
            _mongoDbAccess.GetCollection<TestEntity>().Insert(new TestEntity());
            _mongoDbAccess.GetCollection<TestObject>().Insert(new TestObject());

            _mongoDbAccess.DropCollection<TestObject>();

            _mongoDbAccess.CollectionExists<TestEntity>().Should().BeTrue();
            _mongoDbAccess.CollectionExists<TestObject>().Should().BeFalse();
        }
    }
}