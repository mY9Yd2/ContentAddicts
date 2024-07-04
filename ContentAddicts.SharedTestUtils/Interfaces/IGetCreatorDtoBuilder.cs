using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IGetCreatorDtoBuilder : IBuilderBase<GetCreatorDto>
{
    IGetCreatorDtoBuilder WithId(Guid id);
    IGetCreatorDtoBuilder WithName(string name);
    IGetCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames);
    IGetCreatorDtoBuilder WithSex(Sex sex);
}
