using ContentAddicts.Api.Models;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IGetCreatorDtoBuilder
{
    IGetCreatorDtoBuilder WithId(Guid id);
    IGetCreatorDtoBuilder WithName(string name);
    IGetCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames);
    IGetCreatorDtoBuilder WithSex(Sex sex);
}
