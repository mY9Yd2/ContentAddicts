using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.UnitTests.Fixtures;
using ContentAddicts.UnitTests.Utils;

namespace ContentAddicts.UnitTests.Systems.UseCases.Creators;

public class TestHandlers
        : IClassFixture<AppDbContextFixture>, IClassFixture<AppDbContextFaker>
{
    public AppDbContextFixture Fixture { get; }
    public AppDbContextFaker ContextFaker { get; }

    public TestHandlers(AppDbContextFixture fixture, AppDbContextFaker faker)
    {
        Fixture = fixture;
        ContextFaker = faker;
    }

    [Fact]
    public async Task GetAllCreators_WhenHasCreators_ReturnsListOfCreators()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        await context.Creators.AddAsync(ContextFaker.CreatorFaker.Generate());
        await context.SaveChangesAsync();

        var handler = new GetAllCreatorsHandler(context);
        var query = new GetAllCreatorsQuery();

        var creator = ContextFaker.CreatorFaker.Generate();
        await context.Creators.AddAsync(creator);

        // Act
        var result = await handler.Handle(query, default);

        context.ChangeTracker.Clear();

        // Assert
        result.Should()
                .NotBeNullOrEmpty()
                .And
                .BeAssignableTo<IEnumerable<Creator>>();
    }

    [Fact]
    public async Task GetAllCreators_WhenHasNoCreators_ReturnsEmptyList()
    {
        // Arrange
        using var context = Fixture.CreateContext();

        var handler = new GetAllCreatorsHandler(context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should()
                .NotBeNull()
                .And
                .BeEmpty();
    }

    [Fact]
    public async Task GetCreator_WhenCreatorExists_ReturnsACreator()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var creator = ContextFaker.CreatorFaker.Generate();
        await context.Creators.AddAsync(creator);
        await context.SaveChangesAsync();
        var handler = new GetCreatorHandler(context);
        var query = new GetCreatorQuery(creator.Id);

        context.ChangeTracker.Clear();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should()
                .NotBeNull()
                .And
                .BeOfType<Creator>()
                .And
                .BeEquivalentTo(new
                {
                    creator.Id
                });
    }

    [Fact]
    public async Task GetCreator_WhenNoCreatorExists_ReturnsNull()
    {
        // Arrange
        using var context = Fixture.CreateContext();

        var handler = new GetCreatorHandler(context);
        var query = new GetCreatorQuery(It.IsNotNull<Guid>());

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().BeNull();
    }
}
