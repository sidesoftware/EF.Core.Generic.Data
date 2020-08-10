using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using TestDatabase;

namespace EF.Core.Generic.Data.Tests.TestFixtures
{
    public class SqlLiteWith40ProductsTestFixture : IDisposable
    {
        public TestDbContext Context => SqlLiteInMemoryContext();

        public void Dispose()
        {
            Context?.Dispose();
        }

        private static TestDbContext SqlLiteInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new TestDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.TestCategories.AddRange(TestCategories());
            context.TestProducts.AddRange(TestProducts());
            context.SaveChanges();
            return context;
        }

        private static IEnumerable<TestCategory> TestCategories()
        {
            BuilderSetup.DisablePropertyNamingFor<TestCategory, int>(x => x.Id);
            return Builder<TestCategory>.CreateListOfSize(20).Build().ToList();
        }

        private static IEnumerable<TestProduct> TestProducts()
        {
            BuilderSetup.DisablePropertyNamingFor<TestProduct, int>(x => x.Id);
            var productList = Builder<TestProduct>.CreateListOfSize(40)
                .TheFirst(5)
                .With(x => x.CategoryId = 1)
                .With(x => x.InStock = true)
                .With(x => x.Stock = 300)
                .TheNext(5)
                .With(x => x.InStock = false)
                .With(x => x.CategoryId = 2)
                .With(y => y.Stock = 0)
                .TheRest()
                .With(x => x.CategoryId = 3)
                .With(x => x.InStock = true)
                .With(x => x.Stock = 600)
                .Build();

            return productList.ToList();
        }
    }
}