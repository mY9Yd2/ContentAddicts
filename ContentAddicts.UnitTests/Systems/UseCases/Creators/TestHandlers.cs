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
        IClassFixture<AppDbContextFixture>
{
    private readonly AppDbContextFixture _fixture;

    public TestHandlers(AppDbContextFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllCreators_WhenHasCreators_ReturnsListOfCreators()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var builder = new CreatorBuilder();

        await context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().GetCreator());
        await context.SaveChangesAsync();

        var sut = new GetAllCreatorsHandler(context);
        var query = new GetAllCreatorsQuery();

        // Act
        var result = await sut.Handle(query, default);

        context.ChangeTracker.Clear();

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
        using var context = _fixture.CreateContext();

        var sut = new GetAllCreatorsHandler(context);
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
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var builder = new CreatorBuilder();
        var exceptedCreator = builder.BuildRandomCreator<CreatorBuilder>().GetCreator();

        await context.Creators.AddAsync(builder.BuildRandomCreator<CreatorBuilder>().GetCreator());
        await context.Creators.AddAsync(exceptedCreator);
        await context.SaveChangesAsync();

        var sut = new GetCreatorHandler(context);
        var query = new GetCreatorQuery(exceptedCreator.Id);

        context.ChangeTracker.Clear();

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
        using var context = _fixture.CreateContext();

        var sut = new GetCreatorHandler(context);
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
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var sut = new CreateCreatorHandler(context);
        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var exceptedCreator = createCreatorDtoBuilder
                .BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>()
                .GetCreateCreatorDto();
        var otherNameBuilder = new OtherNameBuilder();
        var command = new CreateCreatorCommand()
        {
            Id = exceptedCreator.Id,
            Name = exceptedCreator.Name,
            OtherNames = [
                otherNameBuilder.BuildRandomOtherName<OtherNameBuilder>()
                        .GetOtherName().Name,
                otherNameBuilder.BuildRandomOtherNameUnicode<OtherNameBuilder>()
                        .GetOtherName().Name
            ],
            Sex = exceptedCreator.Sex
        };

        // Act
        var result = await sut.Handle(command, default);

        context.ChangeTracker.Clear();

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
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().GetCreator();

        await context.Creators.AddAsync(creator);
        await context.SaveChangesAsync();

        var sut = new CreateCreatorHandler(context);
        var command = new CreateCreatorCommand()
        {
            Id = creator.Id,
            Name = creator.Name,
            Sex = creator.Sex
        };

        // Act
        var result = await sut.Handle(command, default);

        context.ChangeTracker.Clear();

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
        using var context = _fixture.CreateContext();
        await context.Database.BeginTransactionAsync();

        var builder = new CreatorBuilder();
        var creator = builder.BuildRandomCreator<CreatorBuilder>().GetCreator();

        await context.Creators.AddAsync(creator);
        await context.SaveChangesAsync();

        var sut = new DeleteCreatorHandler(context);
        var command = new DeleteCreatorCommand(creator.Id);

        // Act
        var result = await sut.Handle(command, default);

        context.ChangeTracker.Clear();

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
        using var context = _fixture.CreateContext();

        var createCreatorDtoBuilder = new CreateCreatorDtoBuilder();
        var creator = createCreatorDtoBuilder
                .BuildRandomCreateCreatorDto<CreateCreatorDtoBuilder>()
                .GetCreateCreatorDto();
        var sut = new DeleteCreatorHandler(context);
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
