using System;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class UpdateTests : IDisposable
    {
        public UpdateTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldAddMultipleRepositoryTypes()
        {
            const string testNameChange = "Test Product Name Change";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var prod = await repo.SingleOrDefaultAsync(x => x.Id == 1);
            prod.Name = testNameChange;

            var repo2 = uow.Repository<TestProduct>();
            repo2.Update(prod);

            await uow.CommitAsync();

            var prod2 = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.Equal(testNameChange, prod2.Name);
        }

        [Fact]
        public void ShouldThrowInvalidOperationException()
        {
            const string newProductName = "Foo Bar";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 1, enableTracking: false);

            Assert.IsAssignableFrom<TestProduct>(product);

            product.Name = newProductName;

            Assert.Throws<InvalidOperationException>(() => repo.Update(product));
        }

        [Fact]
        public void ShouldUpdateMultipleProductsByParams()
        {
            const string newProduct1Name = "Foo Bar";
            const string newProduct2Name = "Bar Foo";

            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product1 = repo.SingleOrDefault(x => x.Id == 1);
            var product2 = repo.SingleOrDefault(x => x.Id == 2);

            product1.Name = newProduct1Name;
            product2.Name = newProduct2Name;

            repo.Update(product1, product2);

            uow.Commit();

            var updatedProduct1 = repo.SingleOrDefault(x => x.Id == 1);
            var updatedProduct2 = repo.SingleOrDefault(x => x.Id == 2);
            Assert.Equal(updatedProduct1.Name, newProduct1Name);
            Assert.Equal(updatedProduct2.Name, newProduct2Name);
        }

        [Fact]
        public void ShouldUpdateProductName()
        {
            const string newProductName = "Foo Bar";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 1);

            Assert.IsAssignableFrom<TestProduct>(product);

            product.Name = newProductName;

            repo.Update(product);

            uow.Commit();

            var updatedProduct = repo.SingleOrDefault(x => x.Id == 1);

            Assert.Equal(updatedProduct.Name, newProductName);
        }

        [Fact]
        public async Task ShouldUpdateWithSameRepository()
        {
            const string testNameChange = "Test Product Name Change";
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var prod = await repo.SingleOrDefaultAsync(x => x.Id == 1);
            prod.Name = testNameChange;

            repo.Update(prod);

            await uow.CommitAsync();

            var prod2 = await repo.SingleOrDefaultAsync(x => x.Id == 1);

            Assert.Equal(testNameChange, prod2.Name);
        }
    }
}