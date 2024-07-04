using ContentAddicts.Api.Models;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface ICreatorBuilder
{
    ICreatorBuilder WithId(Guid id);
    ICreatorBuilder WithName(string name);
    ICreatorBuilder WithOtherNames(HashSet<OtherName> otherNames);
    ICreatorBuilder WithSex(Sex sex);
}
