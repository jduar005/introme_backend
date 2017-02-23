using System;

using FluentAssertions;
using Intro.IntegrationTests.Domain;
using NUnit.Framework;

namespace Intro.IntegrationTests.DataAccess
{
    [TestFixture]
    public class MongoRepositoryNonEntityTests : MongoRepositoryTestBase<TestObject, Guid>
    {
        [Test]
        public void AddAndGetById_WorkAsExpected()
        {
            var entity = Repository.Add(new TestObject { Value = 42 });

            var result = Repository.GetById(entity.Id);

            result.Id.Should().Be(entity.Id);
            result.Value.Should().Be(entity.Value);
        }

        [Test]
        public void Delete_ById_WorksAsExpected()
        {
            var entity = Repository.Add(new TestObject { Value = 42 });

            Repository.Delete(entity.Id);

            Repository.GetById(entity.Id).Should().BeNull();
        }
    }
}