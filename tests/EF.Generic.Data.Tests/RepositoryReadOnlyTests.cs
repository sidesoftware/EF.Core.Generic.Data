using System;
using System.Collections.Generic;
using EF.Core.Generic.Data.Interface;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class RepositoryReadOnlyTests : IDisposable
    {
        public RepositoryReadOnlyTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public void ShouldGetItems()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 1);

            Assert.NotNull(product);
        }

        [Fact]
        public void ShouldGetListOfProducts()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var products = repo.GetList();

            Assert.NotNull(products);
        }

        [Fact]
        public void ShouldReturnAListOfProducts()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var products = repo.GetList(x => x.CategoryId == 1);

            Assert.NotNull(products);
            Assert.IsAssignableFrom<IEnumerable<TestProduct>>(products.Items);
        }

        [Fact]
        public void ShouldReturnInstanceIfInterface()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            Assert.IsAssignableFrom<IRepository<TestProduct>>(repo);
        }

        [Fact]
        public void ShouldReturnNullObject()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var product = repo.SingleOrDefault(x => x.Id == 10001);

            Assert.Null(product);
        }
    }
}