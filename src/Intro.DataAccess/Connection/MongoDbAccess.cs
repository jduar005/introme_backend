using System.Linq;
using Intro.Utility.Extensions;
using MongoDB.Driver;

namespace Intro.DataAccess.Connection
{
    public interface IMongoDbAccess
    {
        MongoDatabase GetDatabase();

        MongoCollection<T> GetCollection<T>();

        bool CollectionExists<T>();

        void DropAllCollections();

        void DropCollection<T>();
    }

    public class MongoDbAccess : IMongoDbAccess
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public MongoDbAccess(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        private static MongoServer GetMongoServer(MongoUrl mongoUrl)
        {
            var client = new MongoClient(mongoUrl);
            var server = client.GetServer(); // tODO: update to new mongo driver usage

            return server;
        }

        public MongoDatabase GetDatabase()
        {
            var mongoUrl = new MongoUrl(connectionStringProvider.GetConnectionString());
            var server = GetMongoServer(mongoUrl);
            var database = server.GetDatabase(mongoUrl.DatabaseName);

            return database;
        }

        public MongoCollection<T> GetCollection<T>()
        {
            var database = GetDatabase();

            return database.GetCollection<T>(typeof(T).Name); // WriteConcern defaulted to Acknowledged
        }

        public bool CollectionExists<T>()
        {
            var database = GetDatabase();

            return database.CollectionExists(typeof(T).Name);
        }

        public void DropAllCollections()
        {
            var database = GetDatabase();
            database.GetCollectionNames().Where(collectionName => !collectionName.Contains("system")).Each(collectionName => database.DropCollection(collectionName));
        }

        public void DropCollection<T>()
        {
            GetCollection<T>().Drop();
        }
    }
}
