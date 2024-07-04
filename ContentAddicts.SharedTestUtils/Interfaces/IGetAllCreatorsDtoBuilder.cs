using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IGetAllCreatorsDtoBuilder : IBuilderBase<GetAllCreatorsDto>
{
    IGetAllCreatorsDtoBuilder WithId(Guid id);
    IGetAllCreatorsDtoBuilder WithName(string name);
    IGetAllCreatorsDtoBuilder WithOtherNames(HashSet<string> otherNames);
}
