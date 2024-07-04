using ContentAddicts.Api.Models;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface ICreateCreatorDtoBuilder
{
    ICreateCreatorDtoBuilder WithId(Guid id);
    ICreateCreatorDtoBuilder WithName(string name);
    ICreateCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames);
    ICreateCreatorDtoBuilder WithSex(Sex sex);
}
