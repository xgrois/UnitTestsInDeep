using FluentAssertions;
using System;
using Xunit;

namespace TestingTechniques.Tests.Unit;
public class ValueSamplesTests
{
    private readonly ValueSamples _sut = new();

    public ValueSamplesTests()
    {

    }

    [Fact]
    public void String_Should_When()
    {
        // Arrange
        var expected = "Peter Parker";

        // Act
        var fullName = _sut.FullName;

        // Assert
        fullName.Should().Be(expected);
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Peter");
        fullName.Should().EndWith("Parker");
    }

    [Fact]
    public void Int_Should_When()
    {
        // Arrange
        var expected = 21;

        // Act
        var age = _sut.Age;

        // Assert
        age.Should().Be(expected);
        age.Should().BePositive();
        age.Should().BeLessThanOrEqualTo(30);
        age.Should().BeInRange(0, 100);
    }

    [Fact]
    public void DateOnly_Should_When()
    {
        // Arrange
        var expected = new DateOnly(2000, 6, 9);

        // Act
        var dateOfBirth = _sut.DateOfBirth;

        // Assert
        dateOfBirth.Should().Be(expected);
        dateOfBirth.Should().BeInRange(new(1999, 10, 10), new(2010, 10, 10));
    }

    [Fact]
    public void Struct_Should_When()
    {
        // Arrange
        var expected = new Point(10.0, 10.0);

        // Act
        var point = _sut.Point;

        // Assert
        point.Should().Be(expected);
    }

    [Fact]
    public void Obj_Should_When()
    {
        // Arrange
        User expected = new()
        {
            FullName = "Peter Parker",
            Age = 21,
            DateOfBirth = new(2000, 6, 9)
        };

        // Act
        var appUser = _sut.AppUser;

        // Assert
        appUser.Should().BeEquivalentTo(expected); // FluentAssertions will map each and every field
    }


    [Fact]
    public void IEnumerableOfInt_ShouldContain_When()
    {
        // Arrange
        var expected = 1;

        // Act
        var numbers = _sut.Numbers.As<int[]>();

        // Assert
        numbers.Should().Contain(expected);
    }

    [Fact]
    public void IEnumerableOfStruct_ShouldContain_When()
    {
        // Arrange
        var expected = new Point(10.0, 10.0);

        // Act
        var points = _sut.Points.As<Point[]>();

        // Assert
        points.Should().Contain(expected);
    }

    [Fact]
    public void IEnumerableOfObj_ShouldContainEquivalentOf_When()
    {
        // Arrange
        var expected = new User()
        {
            FullName = "Peter Parker",
            Age = 21,
            DateOfBirth = new(2000, 6, 9)
        };

        // Act
        var users = _sut.Users.As<User[]>();

        // Assert
        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(u => u.FullName.StartsWith("Peter") && u.Age > 20);
    }

    [Fact]
    public void IEnumerableOfInt_ShouldBe_When()
    {
        // Arrange
        var expected = new[] { 1, 2, 3 };

        // Act
        var numbers = _sut.Numbers.As<int[]>();

        // Assert
        numbers.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void IEnumerableOfStruct_ShouldBeEquivalentTo_When()
    {
        // Arrange
        var expected = new[]
        {
            new Point(10.0, 10.0),
            new Point(20.0,20.0)
        };

        // Act
        var points = _sut.Points.As<Point[]>();

        // Assert
        points.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void IEnumerableOfObj_ShouldBeEquivalentTo_When()
    {
        // Arrange
        var expected = new[]
        {
            new User()
            {
                FullName = "Peter Parker",
                Age = 21,
                DateOfBirth = new(2000, 6, 9)
            },
            new User()
            {
                FullName = "Robert Smith",
                Age = 37,
                DateOfBirth = new(1984, 6, 9)
            },
            new User()
            {
                FullName = "Babara Dopstein",
                Age = 43,
                DateOfBirth = new(1978, 10, 5)
            },
        };

        // Act
        var users = _sut.Users.As<User[]>();

        // Assert
        users.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Event_ShouldRaise_WhenTriggered()
    {
        // FluentAssetions simplifies event testing

        var monitorSubject = _sut.Monitor();

        _sut.RaiseExampleEvent();

        monitorSubject.Should().Raise("ExampleEvent");
    }

    [Fact]
    public void PrivateMethods_ShouldBeTestedIndirectly_WhenParentPublicMethodIsTested()
    {
    }

    [Fact]
    public void InternalMembers_Should_When()
    {
        // You can access internal members if you add this ItemGroup to
        // the project under test
        // < ItemGroup >
        //     < InternalsVisibleTo Include = "TestingTechniques.Tests.Unit" />
        // </ ItemGroup >

        // Now you can access internal member
        // _sut.InternalSecretNumber
    }
}
