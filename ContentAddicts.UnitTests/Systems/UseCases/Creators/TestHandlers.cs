using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.UnitTests.Fixtures;
using ContentAddicts.UnitTests.Utils;

namespace ContentAddicts.UnitTests.Systems.UseCases.Creators;

public class TestHandlers :
        IClassFixture<AppDbContextFixture>,
        IClassFixture<AppDbContextFaker>
{
    private readonly AppDbContextFixture _fixture;
    private readonly AppDbContextFaker _contextFaker;

    public TestHandlers(AppDbContextFixture fixture, AppDbContextFaker faker)
    {
        _fixture = fixture;
        _contextFaker = faker;
    }

    [Fact]
    public async Task GetAllCreators_WhenHasCreators_ReturnsListOfCreators()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        await context.Creators.AddAsync(_contextFaker.CreateCreatorFaker.Generate().ToCreator());
        await context.SaveChangesAsync();

        var sut = new GetAllCreatorsHandler(context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await sut.Handle(query, default);

        context.ChangeTracker.Clear();

        // Assert
        result.Should()
                .NotBeNullOrEmpty()
                .And
                .BeAssignableTo<IEnumerable<GetAllCreatorsDto>>();
    }

    [Fact]
    public async Task GetAllCreators_WhenHasNoCreators_ReturnsEmptyList()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = new GetAllCreatorsHandler(context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await sut.Handle(query, default);

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
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var exceptedDto = _contextFaker.GetCreatorFaker.Generate();
        await context.Creators.AddAsync(exceptedDto.ToCreator());
        await context.SaveChangesAsync();
        var sut = new GetCreatorHandler(context);
        var query = new GetCreatorQuery(exceptedDto.Id);

        context.ChangeTracker.Clear();

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should()
                .NotBeNull()
                .And
                .BeOfType<GetCreatorDto>()
                .And
                .BeEquivalentTo(exceptedDto);
    }

    [Fact]
    public async Task GetCreator_WhenNoCreatorExists_ReturnsNull()
    {
        // Arrange
        using var context = _fixture.CreateContext();

        var sut = new GetCreatorHandler(context);
        var query = new GetCreatorQuery(It.IsNotNull<Guid>());

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var sut = new CreateCreatorHandler(context);
        var exceptedCreator = _contextFaker.CreateCreatorFaker.Generate();
        var query = new CreateCreatorCommand()
        {
            Id = exceptedCreator.Id
        };

        // Act
        var result = await sut.Handle(query, default);

        context.ChangeTracker.Clear();

        // Assert
        result.Should()
                .NotBeNull()
                .And
                .BeOfType<GetCreatorDto>()
                .And
                .BeEquivalentTo(exceptedCreator);
    }
}
