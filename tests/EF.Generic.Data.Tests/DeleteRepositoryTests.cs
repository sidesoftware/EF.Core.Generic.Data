using System;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class DeleteRepositoryTests : IDisposable
    {
        private readonly SqlLiteWith20ProductsTestFixture _fixture;
        public DeleteRepositoryTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void ShouldDeleteProduct()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var repo = uow.Repository<TestProduct>();

            uow.Commit();

            var prod = repo.SingleOrDefault(x => x.Id == 1);
            repo.Remove(prod);
            uow.Commit();

            prod = repo.SingleOrDefault(x => x.Id == 1);
            Assert.Null(prod);
        }
        
        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}