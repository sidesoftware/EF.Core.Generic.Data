using System;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class GetPagedListTest : IDisposable
    {
        public GetPagedListTest(SqlLiteWith20ProductsTestFixture fixture)
        {
            _testFixture = fixture;
        }

        public void Dispose()
        {
            _testFixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _testFixture;

        [Fact]
        public void GetProductPagedListUsingPredicateTest()
        {
            //Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(x => x.CategoryId == 1).Items;
            //Assert
            Assert.Equal(5, productList.Count);
        }

        [Fact]
        public void ShouldGet5ProductsOutOfStockMultiPredicateTest()
        {
            // Arrange
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act
            var productList = repo.GetList(x => x.Stock == 0 && x.InStock.Value == false).Items;
            //Assert
            Assert.Equal(5, productList.Count);
        }

        [Fact]
        public void ShouldReadFromProducts()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();
            //Act 
            var products = repo.GetList().Items;
            //Assert
            Assert.Equal(20, products.Count);
        }

        [Fact]
        public void ShouldGetListWithSelectedColumns()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_testFixture.Context);
            var repo = uow.Repository<TestProduct>();

              var list =  repo.GetList(s => new
              {
                  ProductName = s.Name,
                  StockLevel = s.Stock
              });
              
              Assert.NotNull(list);
              Assert.Equal(20, list.Items.Count);
        }
    }
}