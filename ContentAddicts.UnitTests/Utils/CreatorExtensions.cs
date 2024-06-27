using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.UnitTests.Utils;

public static class CreatorExtensions
{
    public static Creator ToCreator(this GetCreatorDto dto)
    {
        return new Creator()
        {
            Id = dto.Id
        };
    }

    public static Creator ToCreator(this CreateCreatorDto dto)
    {
        return new Creator()
        {
            Id = dto.Id
        };
    }
}
