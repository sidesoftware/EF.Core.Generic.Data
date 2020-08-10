using System;
using System.Linq;
using EF.Core.Generic.Data.Tests.TestFixtures;
using Microsoft.EntityFrameworkCore;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.Product40Collection)]
    public class GetRepositoryTests : IDisposable
    {
        public GetRepositoryTests(SqlLiteWith40ProductsTestFixture fixture)
        {
            _testFixture = fixture;
        }

        public void Dispose()
        {
            _testFixture?.Dispose();
        }

        private readonly SqlLiteWith40ProductsTestFixture _testFixture;

        [Fact]
        public void GetProductPagedListPaginate()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(size: 5);
            //Assert
            Assert.Equal(5, productList.Items.Count);
            Assert.Equal(8, productList.Pages);
            Assert.Equal(5, productList.Size);
            Assert.True(productList.HasNext);
        }

        [Fact]
        public void GetProductPagedListUsingWithNoPredicateTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(size: int.MaxValue).Items;
            //Assert
            Assert.Equal(40, productList.Count);
        }

        [Fact]
        public void GetProductPagedListWith8PagesTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(size: 5);
            //Assert
            Assert.Equal(5, productList.Items.Count);
            Assert.Equal(8, productList.Pages);
        }

        [Fact]
        public void GetProductPagedListWithPredicateTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(x => x.CategoryId == 1);
            //Assert
            Assert.Equal(5, productList.Items.Count);
            Assert.Equal(1, productList.Pages);
        }

        [Fact]
        public void GetProductSingleOrDefaultOrderByTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);

            //Act
            var product = uow.Repository<TestProduct>().SingleOrDefault(orderBy: x => x.OrderBy(x => x.Name),
                include: x => x.Include(x => x.Category));
            //Assert
            Assert.NotNull(product);
            Assert.Equal("Name1", product.Name);
            Assert.IsAssignableFrom<TestProduct>(product);
            Assert.IsAssignableFrom<TestCategory>(product.Category);
        }

        [Fact]
        public void GetProductSingleOrDefaultTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);

            //Act
            var product = uow.Repository<TestProduct>().SingleOrDefault(x => x.Id == 1);
            //Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
        }

        [Fact]
        public void GetProductTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);

            //Act
            var product = uow.Repository<TestProduct>().Get(1);
            //Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
        }
    }
}