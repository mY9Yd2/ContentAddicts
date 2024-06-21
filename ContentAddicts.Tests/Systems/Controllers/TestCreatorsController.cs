using ContentAddicts.Api.Controllers;
using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.Tests.Fixtures;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.Tests.Systems.Controllers;

[Collection("Creators collection")]
public class TestCreatorsController
{
    public CreatorsFixture Fixture { get; }

    public TestCreatorsController(CreatorsFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(Fixture.CreatorsTestData);

        var sut = new CreatorsController(mockMediatr.Object);

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
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync([]);

        var sut = new CreatorsController(mockMediatr.Object);

        // Act
        await sut.GetCreators();

        // Assert
        mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(new GetAllCreatorsQuery(), default),
                    Times.Once()
                );
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync(Fixture.CreatorsTestData);

        var sut = new CreatorsController(mockMediatr.Object);

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
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetAllCreatorsQuery(), default))
                .ReturnsAsync([]);

        var sut = new CreatorsController(mockMediatr.Object);

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
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new Creator());

        var sut = new CreatorsController(mockMediatr.Object);
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
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new Creator());

        var sut = new CreatorsController(mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        await sut.GetCreator(id);

        // Assert
        mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default),
                    Times.Once
                );
    }

    [Fact]
    public async Task GetCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        var mockMediatr = new Mock<IMediator>();

        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync(new Creator());

        var sut = new CreatorsController(mockMediatr.Object);
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
        var mockMediatr = new Mock<IMediator>();
        mockMediatr
                .Setup(mediatr => mediatr.Send(new GetCreatorQuery(It.IsNotNull<Guid>()), default))
                .ReturnsAsync((Creator?)null);

        var sut = new CreatorsController(mockMediatr.Object);
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
