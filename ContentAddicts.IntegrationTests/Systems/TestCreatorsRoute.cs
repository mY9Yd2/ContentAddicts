using System.Net;
using System.Net.Http.Json;

using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.IntegrationTests.Fixtures;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Directors;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAddicts.IntegrationTests.Systems;

public class TestCreatorsRoute :
        IClassFixture<ContentAddictsWebApplicationFactoryFixture<Program>>
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

        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await context.Creators.AddAsync(exceptedCreator);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/creators");
        var content = response.Content
                .ReadFromJsonAsAsyncEnumerable<GetAllCreatorsDto>()
                .ToBlockingEnumerable()
                .ToList();

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        content.Should()
                .ContainSingle()
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

        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await context.Creators.AddAsync(exceptedCreator);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/creators/{exceptedCreator.Id}");
        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

        content.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var exceptedCreator = createCreatorDtoBuilder.BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>().Build();

        // Act
        var response = await _client.PostAsJsonAsync("/api/creators", exceptedCreator);
        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);

        response.Headers.Location
                .Should()
                .NotBeNull()
                .And.Subject
                .ToString()
                .Should()
                .EndWith($"/api/creators/{exceptedCreator.Id}");

        content.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator);
    }

    [Fact]
    public async Task DeleteCreator_OnSuccess_ReturnsStatusCode204()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await context.Creators.AddAsync(creator);
        await context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/creators/{creator.Id}");

        await _factory.ResetDatabaseAsync(context);

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
    }
}
