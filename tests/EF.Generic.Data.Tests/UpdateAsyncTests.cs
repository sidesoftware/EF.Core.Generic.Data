using System;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class UpdateAsyncTests : IDisposable
    {
        public UpdateAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldUpdateProductName()
        {
            const string newProductName = "Foo Bar";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.IsAssignableFrom<TestProduct>(product);

            product.Name = newProductName;

            repo.Update(product);

            await uow.CommitAsync();

            var updatedProduct = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.Equal(updatedProduct.Name, newProductName);
        }
    }
}