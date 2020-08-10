using EF.Core.Generic.Data.Tests.TestFixtures;
using Xunit;

namespace EF.Core.Generic.Data.Tests.TestCollections
{
    [CollectionDefinition(GlobalTestStrings.ProductCollectionName)]
    public class ProductCollection : ICollectionFixture<SqlLiteWith20ProductsTestFixture>
    {
    }
}