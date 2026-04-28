using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FuelTracker.API.Models;

namespace FuelTracker.Api.Tests.Integration.Controllers;

public class AuthControllerTests(IntegrationTestFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetCurrentUser_ShouldReturnOk_WhenAuthorized()
    {
        // Act
        var response = await Client.GetAsync("/api/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
        userResponse.Should().NotBeNull();
        userResponse!.Id.Should().Be(Factory.TestUserId);
        userResponse.Username.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenNoToken()
    {
        // Arrange
        var unauthorizedClient = Factory.CreateClient();
        unauthorizedClient.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await unauthorizedClient.GetAsync("/api/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
