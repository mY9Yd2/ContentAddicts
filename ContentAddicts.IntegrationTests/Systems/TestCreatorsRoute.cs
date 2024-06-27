using System.Net;
using System.Net.Http.Json;

using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.IntegrationTests.Fixtures;
using ContentAddicts.IntegrationTests.Utils;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAddicts.IntegrationTests.Systems;

public class TestCreatorsRoute :
        IClassFixture<ContentAddictsWebApplicationFactoryFixture<Program>>,
        IClassFixture<AppDbContextFaker>
{
    private readonly AppDbContextFaker _contextFaker;
    private readonly HttpClient _client;
    private readonly ContentAddictsWebApplicationFactoryFixture<Program> _factory;

    public TestCreatorsRoute(
            ContentAddictsWebApplicationFactoryFixture<Program> factory,
            AppDbContextFaker faker)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _contextFaker = faker;
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var exceptedCreator = _contextFaker.CreateCreatorFaker.Generate();
        await context.Creators.AddAsync(exceptedCreator.ToCreator());
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/creators");

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        var content = response.Content
                .ReadFromJsonAsAsyncEnumerable<GetAllCreatorsDto>()
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

        var exceptedCreator = _contextFaker.CreateCreatorFaker.Generate();
        await context.Creators.AddAsync(exceptedCreator.ToCreator());
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/creators/{exceptedCreator.Id}");

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

        content.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator);
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var exceptedCreator = _contextFaker.CreateCreatorFaker.Generate();

        // Act
        var response = await _client.PostAsJsonAsync("/api/creators", exceptedCreator);

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

        content.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator);
    }
}
