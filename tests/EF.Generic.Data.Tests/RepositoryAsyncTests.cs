using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Core.Generic.Data.Tests.TestFixtures;
using Microsoft.EntityFrameworkCore;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryAsyncTests : IDisposable
    {
        public RepositoryAsyncTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public async Task ShouldGet5ProductsAndIncludeCategoryInfo()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var results = await repo.GetListAsync(t => t.InStock == true && t.CategoryId == 1,
                include: category => category.Include(x => x.Category),
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(1, results.Pages);
            Assert.IsAssignableFrom<TestCategory>(results.Items[0].Category);
            Assert.Equal("Name1", results.Items[0].Category.Name);
        }

        [Fact]
        public async Task ShouldGetFiveProductsInStockOnePage()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var results = await repo.GetListAsync(t => t.InStock == true && t.CategoryId == 1,
                size: 5);

            Assert.Equal(5, results.Items.Count);
            Assert.Equal(1, results.Pages);
        }

        [Fact]
        public async Task ShouldGetListOf20TestProducts()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            // Act
            var testList = await repo.GetListAsync();

            //Assert
            Assert.NotNull(testList);
            Assert.IsAssignableFrom<IEnumerable<TestProduct>>(testList.Items);
            Assert.Equal(20, testList.Items.Count);
        }

        [Fact]
        public async Task ShouldGetListOf20TestProductsAsync()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            // Act
            var testList = await repo.GetAllAsync();

            //Assert
            Assert.NotNull(testList);
            Assert.Equal(20, testList.Count());
        }

        [Fact]
        public async Task ShouldGetSingleValueAsync()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = await repo.SingleOrDefaultAsync(x => x.Id == 3);

            Assert.NotNull(product);
            Assert.IsAssignableFrom<TestProduct>(product);
        }

        [Fact]
        public async Task ShouldGetAsync()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = await repo.GetAsync(3);

            Assert.NotNull(product);
            Assert.IsAssignableFrom<TestProduct>(product);
        }

        [Fact]
        public async Task ShouldGetName()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var name = await repo.GetFieldAsync(x => x.Id == 3, s => s.Name);

            Assert.Equal("Name3", name);
        }
    }
}