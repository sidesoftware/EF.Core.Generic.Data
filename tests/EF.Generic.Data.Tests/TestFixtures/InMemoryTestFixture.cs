﻿using System;
using Microsoft.EntityFrameworkCore;
using TestDatabase;

namespace EF.Core.Generic.Data.Tests.TestFixtures
{
    public class InMemoryTestFixture : IDisposable
    {
        public TestDbContext Context => InMemoryContext();

        public void Dispose()
        {
            Context?.Dispose();
        }

        private static TestDbContext InMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new TestDbContext(options);

            return context;
        }
    }
}