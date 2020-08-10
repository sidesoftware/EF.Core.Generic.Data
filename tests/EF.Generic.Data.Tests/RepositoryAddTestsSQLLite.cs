using System;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection("RepositoryAdd")]
    public class RepositoryAddTestsSqlLite : IDisposable
    {
        public RepositoryAddTestsSqlLite(SqlLiteWithEmptyDataTestFixture fixture)
        {
            _fixture = fixture;
        }


        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWithEmptyDataTestFixture _fixture;

        [Fact]
        public void ShouldAddNewCategory()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestCategory>();
            var newCategory = new TestCategory {Name = GlobalTestStrings.TestProductCategoryName};
            //Act 
            repo.Add(newCategory);
            uow.Commit();
            //Assert
            Assert.Equal(1, newCategory.Id);
        }

        [Fact]
        public void ShouldAddNewProduct()
        {
            //Arrange
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();
            var newProduct = new TestProduct
            {
                Name = GlobalTestStrings.TestProductName,
                Category = new TestCategory {Id = 1, Name = GlobalTestStrings.TestProductCategoryName}
            };

            //Act 
            repo.Add(newProduct);
            uow.Commit();

            //Assert
            Assert.Equal(1, newProduct.Id);
        }
    }
}