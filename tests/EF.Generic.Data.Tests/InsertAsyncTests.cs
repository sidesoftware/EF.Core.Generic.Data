using System;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Tests.TestFixtures;
using FizzWare.NBuilder;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class InsertAsyncTests : IDisposable
    {
        public InsertAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldInsertNewProductReturnCreatedEntity()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);
            var prod = Builder<TestProduct>.CreateNew().With(x => x.Name = "Cool Product").With(x => x.CategoryId = 1)
                .Build();
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var repo = uow.Repository<TestProduct>();

            var newProduct = await repo.AddAsync(prod);
            await uow.CommitAsync();

            Assert.NotNull(newProduct);
            Assert.IsAssignableFrom<TestProduct>(newProduct.Entity);
            Assert.Equal(21, newProduct.Entity.Id);
        }
    }
}