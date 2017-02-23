using System.Linq;

using FluentAssertions;

using Intro.IntegrationTests.Domain;

using NUnit.Framework;

namespace Intro.IntegrationTests.DataAccess
{
    [TestFixture]
    public class MongoRepositoryEntityTests : MongoRepositoryTestBase<TestEntity>
    {
        [Test]
        public void AddAndGetById_WorkAsExpected()
        {
            var entity = this.Repository.Add(new TestEntity { Value = 42 });

            var result = this.Repository.GetById(entity.Id);

            result.Id.Should().Be(entity.Id);
            result.Value.Should().Be(42);
        }

        [Test]
        public void AddMultiple_AndGetAll_WorksAsExpected()
        {
            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };

            this.Repository.Add(new[] { entity1, entity2 });

            var entities = this.Repository.GetAll().ToList();

            entities.Should().Contain(entity => entity.Value == 7)
                .And.Contain(entity => entity.Value == 13);
        }

        [Test]
        public void Update_ReturnsUpdatedEntityAndWorksAsExpected()
        {
            var entity = new TestEntity { Value = 7 };
            this.Repository.Add(entity);

            entity.Value = 13;
            var result = this.Repository.Update(entity);

            result.Should().Be(entity);
            this.Repository.GetById(entity.Id).Value.Should().Be(13);
        }

        [Test]
        public void Update_Multiple_ReturnsUpdatedEntityAndWorksAsExpected()
        {
            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };
            this.Repository.Add(new[] { entity1, entity2 });

            entity1.Value = 14;
            entity2.Value = 26;
            this.Repository.Update(new[] { entity1, entity2 });

            this.Repository.GetById(entity1.Id).Value.Should().Be(14);
            this.Repository.GetById(entity2.Id).Value.Should().Be(26);
        }

        [Test]
        public void Delete_ById_WorksAsExpected()
        {
            var entity = this.Repository.Add(new TestEntity { Value = 42 });

            this.Repository.Delete(entity.Id);

            this.Repository.GetById(entity.Id).Should().BeNull();
        }

        [Test]
        public void Delete_ByEntity_WorksAsExpected()
        {
            var entity = this.Repository.Add(new TestEntity { Value = 42 });

            this.Repository.Delete(entity);

            this.Repository.GetById(entity.Id).Should().BeNull();
        }

        [Test]
        public void Delete_WithPredicate_WorksAsExpected()
        {
            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };
            this.Repository.Add(new[] { entity1, entity2 });

            this.Repository.Delete(e => e.Value == 7);

            this.Repository.GetById(entity1.Id).Should().BeNull();
            this.Repository.GetById(entity2.Id).Should().NotBeNull();
        }

        [Test]
        public void DeleteAll_WorksAsExpected()
        {
            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };
            this.Repository.Add(new[] { entity1, entity2 });

            this.Repository.DeleteAll();

            this.Repository.GetById(entity1.Id).Should().BeNull();
            this.Repository.GetById(entity2.Id).Should().BeNull();
        }

        [Test]
        public void Count_WorksAsExpected()
        {
            this.Repository.Count().Should().Be(0);

            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };
            this.Repository.Add(new[] { entity1, entity2 });

            this.Repository.Count().Should().Be(2);
        }

        [Test]
        public void Exists_WorksAsExpected()
        {
            var entity1 = new TestEntity { Value = 7 };
            var entity2 = new TestEntity { Value = 13 };
            this.Repository.Add(new[] { entity1, entity2 });

            this.Repository.Exists(e => e.Value == 7).Should().BeTrue();

            this.Repository.Delete(entity1.Id);

            this.Repository.Exists(e => e.Value == 7).Should().BeFalse();
        }
    }
}