using System;
using Intro.Domain;
using Intro.Domain.PersistentModels;

namespace Intro.IntegrationTests.Domain
{
    public class TestObject : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public int Value { get; set; }
    }
}