using System.Configuration;

namespace Intro.DataAccess.Connection
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class MongoConnectionStringProvider : IConnectionStringProvider
    {
        private const string MongoConnectionString = "MongoConnectionString";

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[MongoConnectionString].ConnectionString;
        }
    }
}