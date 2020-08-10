using System;
using EF.Core.Generic.Data.Tests.TestFixtures;
using TestDatabase;
using Xunit;

namespace EF.Core.Generic.Data.Tests
{
    [Collection(GlobalTestStrings.ProductCollectionName)]
    public class DeleteTests : IDisposable
    {
        public DeleteTests(SqlLiteWith20ProductsTestFixture fixture)
        {
            _fixture = fixture;
        }
        
        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private readonly SqlLiteWith20ProductsTestFixture _fixture;

        [Fact]
        public void ShouldDeleteProduct()
        {
            using var uow = new UnitOfWork<TestDbContext>(_fixture.Context);

            var get = uow.Repository<TestProduct>();


            uow.Commit();

            var prod = get.SingleOrDefault(x => x.Id == 1);
            get.Remove(prod);
            uow.Commit();

            prod = get.SingleOrDefault(x => x.Id == 1);
            Assert.Null(prod);
        }
    }
}