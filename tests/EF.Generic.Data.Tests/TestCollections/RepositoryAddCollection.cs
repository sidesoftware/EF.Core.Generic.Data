using EF.Core.Generic.Data.Tests.TestFixtures;
using Xunit;

namespace EF.Core.Generic.Data.Tests.TestCollections
{
    [CollectionDefinition("RepositoryAdd")]
    public class RepositoryAddCollection : ICollectionFixture<SqlLiteWithEmptyDataTestFixture>
    {
    }
}