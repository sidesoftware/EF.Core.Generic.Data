using System;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Interface;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class UnitOfWorkTests : IDisposable
    {
        private static SqlLiteWith20ProductsTestFixture _testFixture;

        public UnitOfWorkTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _testFixture = fixture;
        }

        public void Dispose()
        {
            _testFixture?.Dispose();
        }

        [Fact]
        public void GetOrAddRepositoryTests()
        {
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetOrAddRepository(typeof(TestProduct), new Repository<TestProduct>(_testFixture.Context));
            Assert.NotNull(repo);
            Assert.IsAssignableFrom<IRepository<TestProduct>>(repo);
        }

        [Fact]
        public void GetOrAddMultipleRepositoryTests()
        {
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.GetOrAddRepository(typeof(TestProduct), new Repository<TestProduct>(_testFixture.Context));
            Assert.NotNull(repo);
            Assert.IsAssignableFrom<IRepository<TestProduct>>(repo);

            var repo2 = uow.GetOrAddRepository(typeof(TestProduct),
                new Repository<TestProduct>(_testFixture.Context));
            Assert.NotNull(repo2);
            Assert.IsAssignableFrom<IRepository<TestProduct>>(repo2);
        }

        [Fact]
        public void GetDbConnection()
        {
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var connection = uow.GetDbConnection();
            Assert.NotNull(connection);
        }

        [Fact]
        public void CanConnect()
        {
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var connection = uow.CanConnect();
            Assert.True(connection);
        }

        [Fact]
        public async Task CanConnectAsync()
        {
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var connection = await uow.CanConnectAsync();
            Assert.True(connection);
        }
    }
}