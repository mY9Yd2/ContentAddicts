using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Extensions;

public static class CreatorExtensions
{
    public static Creator ToCreator(this GetCreatorDto dto)
    {
        return new Creator()
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public static Creator ToCreator(this CreateCreatorDto dto)
    {
        return new Creator()
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public static GetCreatorDto ToGetCreatorDto(this Creator creator)
    {
        return new GetCreatorDto()
        {
            Id = creator.Id,
            Name = creator.Name
        };
    }
}
