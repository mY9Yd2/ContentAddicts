using System.Net;
using System.Net.Http.Json;

using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Models;
using ContentAddicts.IntegrationTests.Fixtures;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAddicts.IntegrationTests.Systems;

public class TestCreatorsRoute
    : IClassFixture<ContentAddictsWebApplicationFactoryFixture<Program>>
{
    private readonly HttpClient _client;
    private readonly ContentAddictsWebApplicationFactoryFixture<Program> _factory;

    public TestCreatorsRoute(
            ContentAddictsWebApplicationFactoryFixture<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var exceptedCreator = new Creator()
        {
            Id = Guid.NewGuid()
        };
        await context.Creators.AddAsync(exceptedCreator);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/creators");

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        var content = response.Content
                .ReadFromJsonAsAsyncEnumerable<Creator>()
                .ToBlockingEnumerable()
                .ToList();

        content.Should()
                .HaveCount(1)
                .And
                .NotContainNulls()
                .And
                .Contain(c => c != null && c.Id == exceptedCreator.Id);
    }

    [Fact]
    public async Task GetCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var exceptedCreator = new Creator()
        {
            Id = Guid.NewGuid()
        };
        await context.Creators.AddAsync(exceptedCreator);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/creators/{exceptedCreator.Id}");

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Creator>();

        content.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator);
    }
}
