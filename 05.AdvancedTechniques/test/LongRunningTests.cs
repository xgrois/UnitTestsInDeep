using System.Threading.Tasks;

namespace AdvancedTechniques.Tests.Unit;

public class LongRunningTests
{
    //[Fact(Timeout = 2000)] // After 2s test will fail
    public async Task SlowTest()
    {
        await Task.Delay(10000);
    }
}
