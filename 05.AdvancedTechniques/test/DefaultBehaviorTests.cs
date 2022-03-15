using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AdvancedTechniques.Tests.Unit;

/// <summary>
/// Default xUnit test is to create a new class instance per method to test
/// In addition, tests run sequentially, no matter if methods are async
/// NOTE: Tests in different class will run in parallel in xUnit
/// </summary>
public class DefaultBehaviorTests // : IClassFixture<Shared> if you want that all methods here share anything in your Share class
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly ITestOutputHelper _testOutputHelper;
    // private readonly Shared _shared;

    public DefaultBehaviorTests(ITestOutputHelper testOutputHelper) // add Shared shared)
    {
        _testOutputHelper = testOutputHelper;
        //_shared = shared
    }

    [Fact]
    public async Task ExampleTest1()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }

    [Fact]
    public async Task ExampleTest2()
    {
        _testOutputHelper.WriteLine($"The Guid was: {_id}");
        await Task.Delay(2000);
    }
}
