using ContentAddicts.Api.Controllers;
using ContentAddicts.Api.Models;
using ContentAddicts.Api.Services;
using ContentAddicts.Tests.Fixtures;

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
        var mockCreatorsService = new Mock<ICreatorsService>();

        mockCreatorsService
                .Setup(service => service.GetCreators())
                .ReturnsAsync(Fixture.CreatorsTestData);

        var sut = new CreatorsController(mockCreatorsService.Object);

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
        var mockCreatorsService = new Mock<ICreatorsService>();

        mockCreatorsService
                .Setup(service => service.GetCreators())
                .ReturnsAsync([]);

        var sut = new CreatorsController(mockCreatorsService.Object);

        // Act
        await sut.GetCreators();

        // Assert
        mockCreatorsService
                .Verify(service =>
                    service.GetCreators(),
                    Times.Once()
                );
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        var mockCreatorsService = new Mock<ICreatorsService>();

        mockCreatorsService
                .Setup(service => service.GetCreators())
                .ReturnsAsync(Fixture.CreatorsTestData);

        var sut = new CreatorsController(mockCreatorsService.Object);

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
                .BeAssignableTo<IEnumerable<Creator>>();
    }

    [Fact]
    public async Task GetCreators_OnNoCreatorsFound_Returns204()
    {
        // Arrange
        var mockCreatorsService = new Mock<ICreatorsService>();

        mockCreatorsService
                .Setup(service => service.GetCreators())
                .ReturnsAsync([]);

        var sut = new CreatorsController(mockCreatorsService.Object);

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
}
