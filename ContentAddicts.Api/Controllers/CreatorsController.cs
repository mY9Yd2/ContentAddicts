using ContentAddicts.Api.Models;
using ContentAddicts.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace ContentAddicts.Api.Controllers;

[Route("api/creators")]
[ApiController]
public class CreatorsController : ControllerBase
{
    private readonly ICreatorsService _creatorsService;

    public CreatorsController(ICreatorsService creatorsService)
    {
        _creatorsService = creatorsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreators()
    {
        IEnumerable<Creator> creators = await _creatorsService.GetCreators();

        if (!creators.Any()) return NoContent();

        return Ok(creators);
    }

    [HttpGet("{creatorId:guid}")]
    public async Task<ActionResult<Creator>> GetCreator(Guid creatorId)
    {
        Creator? creator = await _creatorsService.GetCreator(creatorId);

        if (creator is null) return NotFound();

        return Ok(creator);
    }
}
