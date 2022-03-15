using Xunit;

namespace AdvancedTechniques.Tests.Unit;


/// <summary>
/// This is to share your "Shared" class between class tests
/// You only need to copy the below attribute to all classes that you want to share the same "Shared" instance
/// </summary>
[CollectionDefinition("My awesome collection fixture")]
public class TestCollectionFixture : ICollectionFixture<Shared>
{

}
