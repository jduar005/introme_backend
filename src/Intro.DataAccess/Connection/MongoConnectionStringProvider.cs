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
            return "mongodb://localhost:27017/Punched";

            // TODO: use configuration file: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration
            //            return ConfigurationManager.ConnectionStrings[MongoConnectionString].ConnectionString;
        }
    }
}