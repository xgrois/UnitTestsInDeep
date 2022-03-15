using System;

namespace AdvancedTechniques.Tests.Unit;

/// <summary>
/// This class can be used if you want to share something for all test methods
/// For instance, instead of creating a db connection per test method, you can create here 
/// a single one, and share it with all
/// This is "fixtures" in xUnit, and it has more sense in Integration tests
/// </summary>
public class Shared : IDisposable
{
    public Guid Id { get; } = Guid.NewGuid(); // This Id will be the same for all classes that inherit : IClassFixture<Shared>

    public Shared()
    {
        // This method will run only once, at the begining of all tests and before the other constructor
    }

    public void Dispose()
    {
        // This method will run only once, at the end of all tests
    }
}
