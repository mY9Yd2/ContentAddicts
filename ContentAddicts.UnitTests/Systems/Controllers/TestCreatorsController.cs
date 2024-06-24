using ContentAddicts.Api.Controllers;
using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.UnitTests.Utils;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.UnitTests.Systems.Controllers;

public class TestCreatorsController : IClassFixture<AppDbContextFaker>
{
    public AppDbContextFaker ContextFaker { get; }
    private readonly Mock<IMediator> _mockMediatr;

    public TestCreatorsController(AppDbContextFaker faker)
    {
        ContextFaker = faker;
        _mockMediatr = new Mock<IMediator>();
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(ContextFaker.CreatorFaker.Generate(1));

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
    public async Task GetCreators_OnSuccess_InvokesCreatorsServiceExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync([]);

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
                .ReturnsAsync(ContextFaker.CreatorFaker.Generate(1));

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
                .BeAssignableTo<IEnumerable<Creator>>()
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
                .ReturnsAsync([]);

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
                .ReturnsAsync(new Creator());

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
    public async Task GetCreator_OnSuccess_InvokesCreatorsServiceExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new Creator());

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
                .ReturnsAsync(new Creator());

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
                .BeAssignableTo<Creator>();
    }

    [Fact]
    public async Task GetCreator_OnNoCreatorFound_Returns404()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync((Creator?)null);

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        var result = await sut.GetCreator(id);

        // Assert
        result.Result
                .Should()
                .BeOfType<NotFoundResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
    }
}
