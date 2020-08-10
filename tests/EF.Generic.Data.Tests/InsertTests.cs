using System;
using EF.Core.Generic.Data.Tests.TestFixtures;
using FizzWare.NBuilder;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class InsertTests : IDisposable
    {
        public InsertTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public void ShouldInsertAndReturnCreatedEntity()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);
            var prod = Builder<TestProduct>.CreateNew()
                .With(x => x.Name = "Cool Product")
                .With(x => x.CategoryId = 1)
                .Build();

            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            var newProduct = repo.Add(prod);
            uow.Commit();

            Assert.NotNull(newProduct);
            Assert.IsAssignableFrom<TestProduct>(newProduct);
            Assert.Equal(21, newProduct.Id);
        }

        [Fact]
        public void ShouldInsertMultipleProductsByList()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);
            var products = Builder<TestProduct>.CreateListOfSize(3)
                .TheFirst(3)
                .With(x => x.CategoryId = 1)
                .Build();

            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            repo.Add(products);
            uow.Commit();
            var numberOfItems = repo.GetList().Count;

            Assert.Equal(23, numberOfItems);
        }

        [Fact]
        public void ShouldInsertMultipleProductsByParams()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);

            var product1 = Builder<TestProduct>.CreateNew().Build();
            var product2 = Builder<TestProduct>.CreateNew().Build();

            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();

            repo.Add(product1, product2);

            uow.Commit();

            var numberOfItems = repo.GetList().Count;

            Assert.Equal(22, numberOfItems);
        }
    }
}