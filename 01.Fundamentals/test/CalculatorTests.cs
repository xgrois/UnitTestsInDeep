using FluentAssertions; // Arguably more human-readable syntax. Provides better output info. Broadly used
using System;
using Xunit;
using Xunit.Abstractions;

namespace CalculatorLibrary.Tests.Unit;

/// <summary>
/// NOTE: xUnit creates a new CalculatorTests instance for each and every test method inside
/// NOTE: Use IAsyncLifetime instead of IDisposable if you need async code
/// You would have to implement 2 methods, InitializeAsync() and DisposeAsync()
/// Note that InitializeAsync() runs after Constructor
/// </summary>
public class CalculatorTests : IDisposable
{
    private readonly Calculator _sut = new();
    private readonly ITestOutputHelper _testOutputHelper;

    /// <summary>
    /// Here all setups before test
    /// </summary>
    /// <param name="testOutputHelper"></param>
    public CalculatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _testOutputHelper.WriteLine("Setup resources...");
    }

    /// <summary>
    /// Here clean everything after test
    /// </summary>
    public void Dispose()
    {
        _testOutputHelper.WriteLine("Cleaning resources...");
    }

    [Theory]
    [InlineData(5, 4, 9)]
    [InlineData(-5, 4, -1)]
    [InlineData(-1, -1, -2, Skip = "You can skip a particular combination")]
    [InlineData(0, 0, 0)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        _testOutputHelper.WriteLine("Test Add init");
        // Arrage

        // Act
        var actualResult = _sut.Add(a, b);

        // Assert
        //Assert.Equal(expected, actualResult);
        actualResult.Should().Be(expected);

        _testOutputHelper.WriteLine("Test Add completed");
    }



    [Theory]
    [InlineData(5, 5, 0)]
    [InlineData(5, -5, 10)]
    public void Sub_ShouldSubstractTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        _testOutputHelper.WriteLine("Test Sub init");
        // Arrage

        // Act
        var actualResult = _sut.Sub(a, b);

        // Assert
        //Assert.Equal(expected, actualResult);
        actualResult.Should().Be(expected);

        _testOutputHelper.WriteLine("Test Sub completed");
    }

    [Theory]
    [InlineData(2, 5, 10)]
    [InlineData(0, 5, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(0, -5, 0)]
    [InlineData(-5, -5, 25)]
    [InlineData(-5, 5, -25)]
    public void Mul_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        _testOutputHelper.WriteLine("Test Mul init");
        // Arrage

        // Act
        var actualResult = _sut.Mul(a, b);

        // Assert
        //Assert.Equal(expected, actualResult);
        actualResult.Should().Be(expected);

        _testOutputHelper.WriteLine("Test Mul completed");
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(10, 5, 2)]
    public void Div_ShouldDivideTwoNumbers_WhenTwoNumbersAreFloat(float a, float b, float expected)
    {
        _testOutputHelper.WriteLine("Test Div init");
        // Arrage

        // Act
        var actualResult = _sut.Div(a, b);

        // Assert
        //Assert.Equal(expected, actualResult);
        actualResult.Should().Be(expected);

        _testOutputHelper.WriteLine("Test Div completed");
    }

    [Fact(Skip = "Just a test to skip")]
    public void TestToSkip()
    {
    }
}
