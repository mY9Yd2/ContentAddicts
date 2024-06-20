using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators.Get;
using ContentAddicts.Api.UseCases.Creators.GetAll;

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
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreators()
    {
        List<Creator> creators = await _mediatr.Send(new GetAllCreatorsQuery());

        if (creators.Count == 0) return NoContent();

        return Ok(creators);
    }

    [HttpGet("{creatorId:guid}")]
    public async Task<ActionResult<Creator>> GetCreator(Guid creatorId)
    {
        Creator? creator = await _mediatr.Send(new GetCreatorQuery(creatorId));

        if (creator is null) return NotFound();

        return Ok(creator);
    }
}
