using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Delete;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Directors;
using ContentAddicts.UnitTests.Fixtures;

using ErrorOr;

namespace ContentAddicts.UnitTests.Systems.UseCases.Creators;

public class TestHandlers :
        IClassFixture<AppDbContextFixture>,
        IAsyncLifetime
{
    private readonly AppDbContextFixture _fixture;
    private readonly AppDbContext _context;

    public TestHandlers(AppDbContextFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();
    }

    public async Task InitializeAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task GetAllCreators_WhenHasCreators_ReturnsListOfCreators()
    {
        // Arrange
        var builder = new CreatorBuilder();

        await _context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await _context.SaveChangesAsync();

        var sut = new GetAllCreatorsHandler(_context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Value
                .Should()
                .NotBeNullOrEmpty()
                .And
                .BeAssignableTo<IEnumerable<GetAllCreatorsDto>>();
    }

    [Fact]
    public async Task GetAllCreators_WhenHasNoCreators_ReturnsEmptyList()
    {
        // Arrange
        var sut = new GetAllCreatorsHandler(_context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Value
                .Should()
                .NotBeNull()
                .And
                .BeEmpty();
    }

    [Fact]
    public async Task GetCreator_WhenCreatorExists_ReturnsACreator()
    {
        // Arrange
        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().Build());
        await _context.Creators.AddAsync(exceptedCreator);
        await _context.SaveChangesAsync();

        var sut = new GetCreatorHandler(_context);
        var query = new GetCreatorQuery(exceptedCreator.Id);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.IsError
                .Should()
                .BeFalse();

        result.Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType<GetCreatorDto>()
                .And
                .BeEquivalentTo(exceptedCreator, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetCreator_WhenNoCreatorExists_ReturnsNotFoundError()
    {
        // Arrange
        var sut = new GetCreatorHandler(_context);
        var query = new GetCreatorQuery(It.IsNotNull<Guid>());

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.IsError
                .Should()
                .BeTrue();

        result.Errors
                .Should()
                .Contain(e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        var sut = new CreateCreatorHandler(_context);
        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var exceptedCreator = createCreatorDtoBuilder
                .BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>()
                .Build();
        var otherNameBuilder = new OtherNameBuilder();
        var command = new CreateCreatorCommand()
        {
            Id = exceptedCreator.Id,
            Name = exceptedCreator.Name,
            OtherNames = [
                otherNameBuilder.BuildRandomOtherName<OtherNameBuilder>()
                        .Build().Name,
                otherNameBuilder.BuildRandomOtherNameUnicode<OtherNameBuilder>()
                        .Build().Name
            ],
            Sex = exceptedCreator.Sex
        };

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsError
                .Should()
                .BeFalse();

        result.Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType<GetCreatorDto>()
                .And
                .BeEquivalentTo(exceptedCreator, options => options.Excluding(o => o.OtherNames));

        result.Value.OtherNames
                .Should()
                .BeEquivalentTo(command.OtherNames);
    }

    [Fact]
    public async Task CreateCreator_WhenCreatorExists_ReturnsConflictError()
    {
        // Arrange
        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(creator);
        await _context.SaveChangesAsync();

        var sut = new CreateCreatorHandler(_context);
        var command = new CreateCreatorCommand()
        {
            Id = creator.Id,
            Name = creator.Name,
            Sex = creator.Sex
        };

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsError
                .Should()
                .BeTrue();

        result.Errors
                .Should()
                .Contain(e => e.Type == ErrorType.Conflict);
    }

    [Fact]
    public async Task DeleteCreator_WhenCreatorExists_ReturnsDeletedResult()
    {
        // Arrange
        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().Build();

        await _context.Creators.AddAsync(creator);
        await _context.SaveChangesAsync();

        var sut = new DeleteCreatorHandler(_context);
        var command = new DeleteCreatorCommand(creator.Id);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsError
                .Should()
                .BeFalse();

        result.Value
                .Should()
                .Be(Result.Deleted);
    }

    [Fact]
    public async Task DeleteCreator_WhenNoCreatorExists_ReturnsNotFoundError()
    {
        // Arrange
        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var creator = createCreatorDtoBuilder
                .BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>()
                .Build();
        var sut = new DeleteCreatorHandler(_context);
        var command = new DeleteCreatorCommand(creator.Id);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.IsError
                .Should()
                .BeTrue();

        result.Errors
                .Should()
                .Contain(e => e.Type == ErrorType.NotFound);
    }
}
