using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    public class RepositoryAddTest : IClassFixture<InMemoryTestFixture>
    {
        public RepositoryAddTest(InMemoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly InMemoryTestFixture _fixture;

        [Fact]
        public void ShouldAddNewProduct()
        {
            // Arrange 
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);
            var repo = uow.Repository<TestProduct>();
            var newProduct = new TestProduct {Name = GlobalTestStrings.TestProductName};

            // Act
            repo.Add(newProduct);
            uow.Commit();

            //Assert
            Assert.Equal(1, newProduct.Id);
        }
    }
}