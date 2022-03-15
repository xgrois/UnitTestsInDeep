using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Api.Contracts;
using Users.Api.Controllers;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

public class UserControllerTests
{
    private readonly UserController _sut;
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _sut = new UserController(_userService);
    }

    [Fact]
    public async Task GetById_ReturnOkAndObject_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Peter Parker"
        };
        _userService.GetByIdAsync(user.Id).Returns(user);
        var userResponse = user.ToUserResponse();

        // Act (cast is necessary, since IActionResult is just an interface)
        // (The controller returns an .Ok() witch returns OkObjectResult type)
        var result = (OkObjectResult)await _sut.GetById(user.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(userResponse);
    }

    [Fact]
    public async Task GetById_ReturnNotFound_WhenUserDoesntExists()
    {
        // Arrange
        _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userService.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersResponse_WhenUsersExist()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Nick Chapsas"
        };
        var users = new[] { user };
        var usersResponse = users.Select(x => x.ToUserResponse());
        _userService.GetAllAsync().Returns(users);

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEquivalentTo(usersResponse);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedUser_AndStatusCode201_WhenCreateUserRequestIsValid()
    {
        // Arrange
        CreateUserRequest createUserRequest = new CreateUserRequest()
        {
            FullName = "Peter Parker"
        };

        User user = new User
        {
            Id = Guid.NewGuid(),
            FullName = createUserRequest.FullName
        };

        // Since the Controller instantiates its own internal User, the Id will not match
        // with User in this scope. Thus, we need to tell NSubstitute to override the User in this scope
        // to whatever User created inside via Arg.Do<User>(x => user = x)
        _userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true);

        // Act
        var result = (CreatedAtActionResult)await _sut.Create(createUserRequest);

        // Assert
        UserResponse expectedUserResponse = user.ToUserResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedUserResponse);
        result.RouteValues!["id"].Should().BeEquivalentTo(user.Id);

        // You can just exclude Id from checking 
        // result.Value.As<UserResponse>().Should()
        //     .BeEquivalentTo(expectedUserResponse, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Create_ShouldReturnStatusCode400_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        CreateUserRequest createUserRequest = new CreateUserRequest();

        _userService.CreateAsync(Arg.Any<User>()).Returns(false);

        // Act
        var result = (BadRequestResult)await _sut.Create(createUserRequest);

        // Assert
        result.StatusCode.Should().Be(400);
    }


    [Fact]
    public async Task DeleteById_Returns200_WhenUserWasDeleted()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userService.DeleteByIdAsync(userId).Returns(true);

        // Act
        var result = (OkResult)await _sut.DeleteById(userId);

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteById_Returns404_WhenUserWasNotDeleted()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userService.DeleteByIdAsync(userId).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.DeleteById(userId);

        // Assert
        result.StatusCode.Should().Be(404);
    }

}
