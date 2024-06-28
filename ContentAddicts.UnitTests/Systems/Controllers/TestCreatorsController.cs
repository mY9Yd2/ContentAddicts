using ContentAddicts.Api.Controllers;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.UnitTests.Utils;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.UnitTests.Systems.Controllers;

public class TestCreatorsController : IClassFixture<AppDbContextFaker>
{
    private readonly AppDbContextFaker _contextFaker;
    private readonly Mock<IMediator> _mockMediatr;

    public TestCreatorsController(AppDbContextFaker faker)
    {
        _contextFaker = faker;
        _mockMediatr = new Mock<IMediator>();
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(_contextFaker.GetAllCreatorsFaker.Generate(1));

        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.GetCreators();

        // Assert
        result.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetCreators_OnSuccess_SendAQueryExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(new List<GetAllCreatorsDto>());

        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.GetCreators();

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(new GetAllCreatorsQuery(), default),
                    Times.Once()
                );
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(_contextFaker.GetAllCreatorsFaker.Generate(1));

        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.GetCreators();

        // Assert
        result.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.Value
                .Should()
                .NotBeNull()
                .And
                .BeAssignableTo<IEnumerable<GetAllCreatorsDto>>()
                .Which
                .Should()
                .NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetCreators_OnNoCreatorsFound_Returns204()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(new List<GetAllCreatorsDto>());

        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.GetCreators();

        // Assert
        result.Result
                .Should()
                .BeOfType<NoContentResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task GetCreator_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new GetCreatorDto());

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        var result = await sut.GetCreator(id);

        // Assert
        result.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetCreator_OnSuccess_SendAQueryExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new GetCreatorDto());

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        await sut.GetCreator(id);

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default),
                    Times.Once
                );
    }

    [Fact]
    public async Task GetCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new GetCreatorDto());

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        var result = await sut.GetCreator(id);

        // Assert
        result.Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.Value
                .Should()
                .NotBeNull()
                .And
                .BeAssignableTo<GetCreatorDto>();
    }

    [Fact]
    public async Task GetCreator_OnNoCreatorFound_Returns404()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(Error.NotFound());

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        var result = await sut.GetCreator(id);

        // Assert
        result.Result
                .Should()
                .BeOfType<NotFoundObjectResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_Returns201()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(new GetCreatorDto());

        var command = new CreateCreatorCommand();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.CreateCreator(command);

        // Assert
        result.Result
                .Should()
                .BeOfType<CreatedAtActionResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_SendACommandExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(new GetCreatorDto());

        var command = new CreateCreatorCommand();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.CreateCreator(command);

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(command, default),
                    Times.Once()
                );
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        var exceptedCreator = _contextFaker.GetCreatorFaker.Generate();

        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(exceptedCreator);

        var command = new CreateCreatorCommand();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.CreateCreator(command);

        // Assert
        result.Result
                .Should()
                .BeOfType<CreatedAtActionResult>()
                .Which.Value
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(exceptedCreator);
    }

    [Fact]
    public async Task CreateCreator_WhenTheCreatorExists_Returns409()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(Error.Conflict());

        var command = new CreateCreatorCommand();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.CreateCreator(command);

        // Assert
        result.Result
                .Should()
                .BeOfType<ConflictObjectResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status409Conflict);
    }
}
