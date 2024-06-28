using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.Api.UseCases.Creators.Create;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.Api.Controllers;

[Route("api/creators")]
[ApiController]
public class CreatorsController : ControllerBase
{
    private readonly IMediator _mediatr;

    public CreatorsController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllCreatorsDto>>> GetCreators()
    {
        ErrorOr<List<GetAllCreatorsDto>> getAllCreatorsResult = await _mediatr.Send(new GetAllCreatorsQuery());

        if (getAllCreatorsResult.Value.Count == 0)
        {
            return NoContent();
        }

        return Ok(getAllCreatorsResult.Value);
    }

    [HttpGet("{creatorId:guid}")]
    public async Task<ActionResult<GetCreatorDto>> GetCreator(Guid creatorId)
    {
        ErrorOr<GetCreatorDto> getCreatorResult = await _mediatr.Send(new GetCreatorQuery(creatorId));

        if (getCreatorResult.IsError)
        {
            return NotFound(getCreatorResult.Errors);
        }

        return Ok(getCreatorResult.Value);
    }

    [HttpPost]
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
}
