using System.Net.Mime;

using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Delete;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.Api.Controllers;

/// <summary>
/// Operations related to content creators
/// </summary>
[Route("api/creators")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class CreatorsController : ControllerBase
{
    private readonly IMediator _mediatr;

    public CreatorsController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    /// <summary>
    /// Get a list of content creators
    /// </summary>
    /// <remarks>
    /// Retrieve a list of content creators discussed on the platform
    /// </remarks>
    /// <response code="204">No creators available</response>
    /// <response code="200">List of content creators</response>
    /// <returns>A list of content creators</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetAllCreatorsDto>>> GetCreators()
    {
        ErrorOr<List<GetAllCreatorsDto>> getAllCreatorsResult = await _mediatr.Send(new GetAllCreatorsQuery());

        if (getAllCreatorsResult.Value.Count == 0)
        {
            return NoContent();
        }

        return Ok(getAllCreatorsResult.Value);
    }

    /// <summary>
    /// Get a content creator
    /// </summary>
    /// <remarks>
    /// Retrieve a content creator discussed on the platform
    /// </remarks>
    /// <param name="creatorId">Unique identifier for the creator</param>
    /// <response code="404">No content creator found with the provided Id</response>
    /// <response code="200">A content creator</response>
    /// <returns>A content creator</returns>
    [HttpGet("{creatorId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetCreatorDto>> GetCreator(Guid creatorId)
    {
        ErrorOr<GetCreatorDto> getCreatorResult = await _mediatr.Send(new GetCreatorQuery(creatorId));

        if (getCreatorResult.IsError)
        {
            return NotFound(getCreatorResult.Errors);
        }

        return Ok(getCreatorResult.Value);
    }

    /// <summary>
    /// Create a new content creator
    /// </summary>
    /// <remarks>
    /// Creates a new content creator profile
    /// </remarks>
    /// <param name="command">Information that must be provided to register a content creator</param>
    /// <returns>A content creator</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<GetCreatorDto>> CreateCreator(CreateCreatorCommand command)
    {
        ErrorOr<GetCreatorDto> createCreatorResult = await _mediatr.Send(command);

        if (createCreatorResult.IsError)
        {
            return Conflict(createCreatorResult.Errors);
        }

        return CreatedAtAction(
            nameof(GetCreator),
            new { creatorId = createCreatorResult.Value.Id },
            createCreatorResult.Value
        );
    }

    /// <summary>
    /// Delete a content creator
    /// </summary>
    /// <remarks>
    /// Deletes a specific content creator by its Id.
    /// </remarks>
    /// <param name="creatorId">Unique identifier for the creator</param>
    /// <returns></returns>
    [HttpDelete("{creatorId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCreator(Guid creatorId)
    {
        ErrorOr<Deleted> deleteCreatorResult = await _mediatr.Send(new DeleteCreatorCommand(creatorId));

        if (deleteCreatorResult.IsError)
        {
            return NotFound(deleteCreatorResult.Errors);
        }

        return NoContent();
    }
}
