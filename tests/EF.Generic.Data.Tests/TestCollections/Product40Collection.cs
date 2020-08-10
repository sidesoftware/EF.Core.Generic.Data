using EF.Core.Generic.Data.Tests.TestFixtures;
using Xunit;

namespace EF.Core.Generic.Data.Tests.TestCollections
{
    [CollectionDefinition(GlobalTestStrings.Product40Collection)]
    public class Product40Collection : ICollectionFixture<SqlLiteWith40ProductsTestFixture>
    {
    }
}