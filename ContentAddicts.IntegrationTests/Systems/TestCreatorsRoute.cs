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
        IClassFixture<ContentAddictsWebApplicationFactoryFixture<Program>>,
        IClassFixture<ScopedServicesFixture>,
        IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ContentAddictsWebApplicationFactoryFixture<Program> _factory;
    private readonly AppDbContext _context;
    private readonly ScopedServicesFixture _scopedServicesFixture;

    public TestCreatorsRoute(
            ContentAddictsWebApplicationFactoryFixture<Program> factory,
            ScopedServicesFixture scopedServicesFixture)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _scopedServicesFixture = scopedServicesFixture;
        _scopedServicesFixture.Scope = _factory.Services.CreateScope();
        _context = _scopedServicesFixture.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync(_context);

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(exceptedCreator);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/creators");
        var content = response.Content
                .ReadFromJsonAsAsyncEnumerable<GetAllCreatorsDto>()
                .ToBlockingEnumerable()
                .ToList();

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
        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await _context.Creators.AddAsync(exceptedCreator);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/creators/{exceptedCreator.Id}");
        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

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
        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var exceptedCreator = createCreatorDtoBuilder.BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>().Build();

        // Act
        var response = await _client.PostAsJsonAsync("/api/creators", exceptedCreator);
        var content = await response.Content.ReadFromJsonAsync<GetCreatorDto>();

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
        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await _context.Creators.AddAsync(creator);
        await _context.SaveChangesAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/creators/{creator.Id}");

        // Assert
        response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
    }
}
