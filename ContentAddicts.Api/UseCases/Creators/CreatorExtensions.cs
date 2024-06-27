using ContentAddicts.Api.Models;

namespace ContentAddicts.Api.UseCases.Creators;

public static class CreatorExtensions
{
    public static GetCreatorDto ToGetCreatorDto(this Creator creator)
    {
        return new GetCreatorDto()
        {
            Id = creator.Id
        };
    }
}
