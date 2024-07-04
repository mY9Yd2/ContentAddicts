namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IGetAllCreatorsDtoBuilder
{
    IGetAllCreatorsDtoBuilder WithId(Guid id);
    IGetAllCreatorsDtoBuilder WithName(string name);
    IGetAllCreatorsDtoBuilder WithOtherNames(HashSet<string> otherNames);
}
