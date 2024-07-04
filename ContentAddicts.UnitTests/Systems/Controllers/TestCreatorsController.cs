using ContentAddicts.Api.Controllers;
using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Delete;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Directors;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.UnitTests.Systems.Controllers;

public class TestCreatorsController
{
    private readonly Mock<IMediator> _mockMediatr;

    public TestCreatorsController()
    {
        _mockMediatr = new Mock<IMediator>();
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var getAllCreatorsDtoBuilder = new GetAllCreatorsDtoBuilder();

        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetAllCreatorsQuery>(), default))
                .ReturnsAsync(new List<GetAllCreatorsDto>
                        {
                            getAllCreatorsDtoBuilder.BuildRandomGetAllCreatorsDto<GetAllCreatorsDtoBuilder>()
                                    .Build()
                        });

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
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetAllCreatorsQuery>(), default))
                .ReturnsAsync(new List<GetAllCreatorsDto>());

        var query = new GetAllCreatorsQuery();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.GetCreators();

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(query, default),
                    Times.Once());
    }

    [Fact]
    public async Task GetCreators_OnSuccess_ReturnsListOfCreators()
    {
        // Arrange
        var getAllCreatorsDtoBuilder = new GetAllCreatorsDtoBuilder();

        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetAllCreatorsQuery>(), default))
                .ReturnsAsync(new List<GetAllCreatorsDto>
                        {
                            getAllCreatorsDtoBuilder.BuildRandomGetAllCreatorsDto<GetAllCreatorsDtoBuilder>()
                                    .Build()
                        });

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
    public async Task GetCreators_OnNoCreatorsFound_ReturnsStatusCode204()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetAllCreatorsQuery>(), default))
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
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetCreatorQuery>(), default))
                .ReturnsAsync(It.IsNotNull<GetCreatorDto>());

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
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetCreatorQuery>(), default))
                .ReturnsAsync(It.IsNotNull<GetCreatorDto>());

        var id = It.IsNotNull<Guid>();
        var query = new GetCreatorQuery(id);
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.GetCreator(id);

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(query, default),
                    Times.Once());
    }

    [Fact]
    public async Task GetCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetCreatorQuery>(), default))
                .ReturnsAsync(new GetCreatorDto()
                {
                    Id = It.IsNotNull<Guid>(),
                    Name = It.IsNotNull<string>(),
                    Sex = Sex.NotApplicable
                });

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
    public async Task GetCreator_WhenCreatorDoesNotExist_ReturnsStatusCode404()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<GetCreatorQuery>(), default))
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
    public async Task CreateCreator_OnSuccess_ReturnsStatusCode201()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(new GetCreatorDto()
                {
                    Id = It.IsNotNull<Guid>(),
                    Name = It.IsNotNull<string>(),
                    Sex = Sex.NotApplicable
                });

        var command = new CreateCreatorCommand()
        {
            Id = It.IsNotNull<Guid>(),
            Name = It.IsNotNull<string>(),
            Sex = It.IsNotNull<Sex>()
        };
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
                .ReturnsAsync(new GetCreatorDto()
                {
                    Id = It.IsNotNull<Guid>(),
                    Name = It.IsNotNull<string>(),
                    Sex = Sex.NotApplicable
                });

        var command = new CreateCreatorCommand()
        {
            Id = It.IsNotNull<Guid>(),
            Name = It.IsNotNull<string>(),
            Sex = It.IsNotNull<Sex>()
        };
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.CreateCreator(command);

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(command, default),
                    Times.Once());
    }

    [Fact]
    public async Task CreateCreator_OnSuccess_ReturnsACreator()
    {
        // Arrange
        var getCreatorDtoBuilder = new GetCreatorDtoBuilder();
        var exceptedCreator = getCreatorDtoBuilder.BuildRandomGetCreatorDto<GetCreatorDtoBuilder>().Build();

        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(exceptedCreator);

        var command = new CreateCreatorCommand()
        {
            Id = It.IsNotNull<Guid>(),
            Name = It.IsNotNull<string>(),
            Sex = It.IsNotNull<Sex>()
        };
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
    public async Task CreateCreator_WhenTheCreatorExists_ReturnsStatusCode409()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<CreateCreatorCommand>(), default))
                .ReturnsAsync(Error.Conflict());

        var command = new CreateCreatorCommand()
        {
            Id = It.IsNotNull<Guid>(),
            Name = It.IsNotNull<string>(),
            Sex = It.IsNotNull<Sex>()
        };
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

    [Fact]
    public async Task DeleteCreator_OnSuccess_ReturnsStatusCode204()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<DeleteCreatorCommand>(), default))
                .ReturnsAsync(Result.Deleted);

        var sut = new CreatorsController(_mockMediatr.Object);
        var id = It.IsNotNull<Guid>();

        // Act
        var result = await sut.DeleteCreator(id);

        // Assert
        result.Should()
                .BeOfType<NoContentResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteCreator_OnSuccess_SendACommandExactlyOnce()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<DeleteCreatorCommand>(), default))
                .ReturnsAsync(Result.Deleted);

        var id = It.IsNotNull<Guid>();
        var command = new DeleteCreatorCommand(id);
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        await sut.DeleteCreator(id);

        // Assert
        _mockMediatr
                .Verify(mediatr =>
                    mediatr.Send(command, default),
                    Times.Once());
    }

    [Fact]
    public async Task DeleteCreator_WhenCreatorDoesNotExist_ReturnsStatusCode404()
    {
        // Arrange
        _mockMediatr
                .Setup(mediatr => mediatr.Send(It.IsNotNull<DeleteCreatorCommand>(), default))
                .ReturnsAsync(Error.NotFound());

        var id = It.IsNotNull<Guid>();
        var sut = new CreatorsController(_mockMediatr.Object);

        // Act
        var result = await sut.DeleteCreator(id);

        // Assert
        result.Should()
                .BeOfType<NotFoundObjectResult>()
                .Which.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
    }
}
