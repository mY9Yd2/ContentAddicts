using Bogus;

using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Builders;

public class GetAllCreatorsDtoBuilder : IGetAllCreatorsDtoBuilder
{
    private Faker<GetAllCreatorsDto> _faker = null!;

    public GetAllCreatorsDtoBuilder() => Reset();

    public void Reset() => _faker = new Faker<GetAllCreatorsDto>()
            .UseSeed(28532)
            .StrictMode(true);

    public GetAllCreatorsDto Build()
    {
        GetAllCreatorsDto result = _faker.Generate();
        Reset();
        return result;
    }

    public IGetAllCreatorsDtoBuilder WithId(Guid id)
    {
        _faker.RuleFor(c => c.Id, id);
        return this;
    }

    public IGetAllCreatorsDtoBuilder WithName(string name)
    {
        _faker.RuleFor(c => c.Name, name);
        return this;
    }

    public IGetAllCreatorsDtoBuilder WithOtherNames(HashSet<string> otherNames)
    {
        _faker.RuleFor(c => c.OtherNames, (_, c) =>
                {
                    foreach (var otherName in otherNames)
                        c.OtherNames.Add(otherName);
                    return c.OtherNames;
                });
        return this;
    }
}
