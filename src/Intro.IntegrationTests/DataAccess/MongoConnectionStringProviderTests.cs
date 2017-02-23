using FluentAssertions;
using Intro.DataAccess;
using Intro.DataAccess.Connection;
using NUnit.Framework;

namespace Intro.IntegrationTests.DataAccess
{
    [TestFixture]
    public class MongoConnectionStringProviderTests
    {
        [Test]
        public void GetConnectionString_ReturnsMongoServerSettings()
        {
            var provider = new MongoConnectionStringProvider();
            provider.GetConnectionString().Should().Be("mongodb://localhost:27017/IntroTest");
        }
    }
}