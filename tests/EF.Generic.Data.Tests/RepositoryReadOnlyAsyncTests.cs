using System;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Interface;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryReadOnlyAsyncTests : IDisposable
    {
        public RepositoryReadOnlyAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldGetListOfItems()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var results = await repo.GetListAsync(t => t.InStock == true && t.CategoryId == 1,
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(1, results.Pages);
        }

        [Fact]
        public async Task ShouldGetSingleItem()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.NotNull(product);
        }

        [Fact]
        public void ShouldReturnInstanceIfInterface()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            Assert.IsAssignableFrom<IRepository<TestProduct>>(repo);
        }

        [Fact]
        public async Task ShouldReturnNullObject()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 10001);

            Assert.Null(product);
        }
    }
}