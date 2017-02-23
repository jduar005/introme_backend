using Intro.DataAccess;
using Intro.DataAccess.Connection;
using Intro.Domain;
using Intro.Domain.PersistentModels;
using NUnit.Framework;

namespace Intro.IntegrationTests.DataAccess
{
    public abstract class MongoRepositoryTestBase<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        protected MongoRepository<TEntity, TKey> Repository;

        [SetUp]
        public virtual void BeforeEach()
        {
            var connectionStringProvider = new MongoConnectionStringProvider();
            this.Repository = new MongoRepository<TEntity, TKey>(connectionStringProvider);

            new MongoDbAccess(connectionStringProvider).DropCollection<TEntity>();
        }
    }

    public abstract class MongoRepositoryTestBase<T> where T : IEntity<string>
    {
        protected MongoRepository<T> Repository;

        [SetUp]
        public virtual void BeforeEach()
        {
            var connectionStringProvider = new MongoConnectionStringProvider();
            this.Repository = new MongoRepository<T>(connectionStringProvider);

            new MongoDbAccess(connectionStringProvider).DropCollection<T>();
        }
    }
}