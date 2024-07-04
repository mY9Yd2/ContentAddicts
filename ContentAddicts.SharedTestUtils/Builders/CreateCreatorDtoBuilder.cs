using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Builders;

public class CreateCreatorDtoBuilder : ICreateCreatorDtoBuilder
{
    private Faker<CreateCreatorDto> _faker = null!;

    public CreateCreatorDtoBuilder() => Reset();

    public void Reset() => _faker = new Faker<CreateCreatorDto>()
            .UseSeed(5589)
            .StrictMode(true);

    public CreateCreatorDto GetCreateCreatorDto()
    {
        CreateCreatorDto result = _faker.Generate();
        Reset();
        return result;
    }

    public ICreateCreatorDtoBuilder WithId(Guid id)
    {
        _faker.RuleFor(c => c.Id, id);
        return this;
    }

    public ICreateCreatorDtoBuilder WithName(string name)
    {
        _faker.RuleFor(c => c.Name, name);
        return this;
    }

    public ICreateCreatorDtoBuilder WithOtherNames(HashSet<string> otherNames)
    {
        _faker.RuleFor(c => c.OtherNames, (_, c) =>
                {
                    foreach (var otherName in otherNames)
                        c.OtherNames.Add(otherName);
                    return c.OtherNames;
                });
        return this;
    }

    public ICreateCreatorDtoBuilder WithSex(Sex sex)
    {
        _faker.RuleFor(c => c.Sex, sex);
        return this;
    }
}
