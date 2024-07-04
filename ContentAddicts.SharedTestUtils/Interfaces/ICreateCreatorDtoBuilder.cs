using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface ICreateCreatorDtoBuilder : IBuilderBase<CreateCreatorDto>
{
    ICreateCreatorDtoBuilder WithId(Guid id);
    ICreateCreatorDtoBuilder WithName(string name);
    ICreateCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames);
    ICreateCreatorDtoBuilder WithSex(Sex sex);
}
