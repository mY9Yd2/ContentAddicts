using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Builders;

public class GetCreatorDtoBuilder : IGetCreatorDtoBuilder
{
    private Faker<GetCreatorDto> _faker = null!;

    public GetCreatorDtoBuilder() => Reset();

    public void Reset() => _faker = new Faker<GetCreatorDto>()
            .UseSeed(25463)
            .StrictMode(true);

    public GetCreatorDto Build()
    {
        GetCreatorDto result = _faker.Generate();
        Reset();
        return result;
    }

    public IGetCreatorDtoBuilder WithId(Guid id)
    {
        _faker.RuleFor(c => c.Id, id);
        return this;
    }

    public IGetCreatorDtoBuilder WithName(string name)
    {
        _faker.RuleFor(c => c.Name, name);
        return this;
    }

    public IGetCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames)
    {
        _faker.RuleFor(c => c.OtherNames, (_, c) =>
                {
                    foreach (var otherName in otherNames)
                        c.OtherNames.Add(otherName);
                    return c.OtherNames;
                });
        return this;
    }

    public IGetCreatorDtoBuilder WithSex(Sex sex)
    {
        _faker.RuleFor(c => c.Sex, sex);
        return this;
    }
}
