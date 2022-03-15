using FluentAssertions;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

    public UserServiceTests()
    {
        _sut = new UserService(_userRepository, _logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers_WhenSomeUsersExist()
    {
        // Arrange
        var peterParker = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        var expectedUsers = new[]
        {
            peterParker
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        //result.Single().Should().BeEquivalentTo(peterParker);
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange (not really care about what it returns here)
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act (we only care that when this is executed, below logs match)
        await _sut.GetAllAsync();

        // Assert (matching logs can be useful in unit testing, since other services might react based on specific logs)
        _logger.Received(1).LogInformation(Arg.Is("Retrieving all users"));

        // Arg.Any<long>() is used to say any long value in {0} is ok (we dont care specific miliseconds)
        _logger.Received(1).LogInformation(Arg.Is("All users retrieved in {0}ms"), Arg.Any<long>());

        // Alternative asserts log starts with...
        _logger.Received(1).LogInformation(Arg.Is<string>(x => x.StartsWith("Retr")));
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.GetAllAsync()
            .Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.GetAllAsync();

        // Assert
        await requestAction.Should()
            .ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger.Received(1).LogError(Arg.Is(sqliteException), Arg.Is("Something went wrong while retrieving all users"));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAUser_WhenAUserExists()
    {
        // Assert
        User existingUser = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.GetByIdAsync(existingUser.Id).Returns(existingUser);

        // Act
        var result = await _sut.GetByIdAsync(existingUser.Id);

        // Assert
        result.Should().BeEquivalentTo(existingUser);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Assert
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogCorrectMessages_WhenRetrievingTheUser()
    {
        // Arrange (not really care about what it returns here)
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        // Act (we only care that when this is executed, below logs match)
        await _sut.GetByIdAsync(user.Id);

        // Assert (matching logs can be useful in unit testing, since other services might react based on specific logs)
        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("Retrieving user with id: {0}"),
                Arg.Is<Guid>(user.Id));

        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("User with id {0} retrieved in {1}ms"),
                Arg.Is<Guid>(user.Id),
                Arg.Any<long>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogCorrectMessages_WhenExceptionIsThrown()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.GetByIdAsync(user.Id)
            .Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.GetByIdAsync(user.Id);

        // Assert
        await requestAction.Should()
            .ThrowAsync<SqliteException>()
            .WithMessage("Something went wrong");

        _logger
            .Received(1)
            .LogError(
                Arg.Is(sqliteException),
                "Something went wrong while retrieving user with id {0}",
                Arg.Is<Guid>(user.Id));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenUserDetailsAreValid()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.CreateAsync(user).Returns(true);

        // Act
        var result = await _sut.CreateAsync(user);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldLogCorrectMessages_WhenCreatingAUser()
    {
        // Arrange (not really care about what it returns here)
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.CreateAsync(user).Returns(true);

        // Act (we only care that when this is executed, below logs match)
        await _sut.CreateAsync(user);

        // Assert (matching logs can be useful in unit testing, since other services might react based on specific logs)
        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("Creating user with id {0} and name: {1}"),
                Arg.Is<Guid>(user.Id),
                Arg.Is<string>(user.FullName));

        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("User with id {0} created in {1}ms"),
                Arg.Is<Guid>(user.Id),
                Arg.Any<long>());
    }

    [Fact]
    public async Task CreateAsync_ShouldLogCorrectMessages_WhenExceptionIsThrown()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.CreateAsync(user)
            .Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.CreateAsync(user);

        // Assert
        await requestAction.Should()
            .ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger
            .Received(1)
            .LogError(
                Arg.Is(sqliteException),
                "Something went wrong while creating a user");
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserIdIsValid()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.DeleteByIdAsync(user.Id).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(user.Id);

        // Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldNotDeleteUser_WhenUserDoesNotExists()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.DeleteByIdAsync(user.Id).Returns(false);

        // Act
        var result = await _sut.DeleteByIdAsync(user.Id);

        // Assert
        result.Should().Be(false);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogCorrectMessages_WhenDeletingAUser()
    {
        // Arrange (not really care about what it returns here)
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userRepository.DeleteByIdAsync(user.Id).Returns(true);

        // Act (we only care that when this is executed, below logs match)
        await _sut.DeleteByIdAsync(user.Id);

        // Assert (matching logs can be useful in unit testing, since other services might react based on specific logs)
        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("Deleting user with id: {0}"),
                Arg.Is<Guid>(user.Id));

        _logger
            .Received(1)
            .LogInformation(
                Arg.Is("User with id {0} deleted in {1}ms"),
                Arg.Is<Guid>(user.Id),
                Arg.Any<long>());
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogCorrectMessages_WhenExceptionIsThrown()
    {
        // Arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.DeleteByIdAsync(user.Id)
            .Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.DeleteByIdAsync(user.Id);

        // Assert
        await requestAction.Should()
            .ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger
            .Received(1)
            .LogError(
                Arg.Is(sqliteException),
                "Something went wrong while deleting user with id {0}",
                Arg.Is<Guid>(user.Id));
    }

}
