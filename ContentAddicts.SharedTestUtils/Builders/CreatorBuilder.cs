using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Builders;

public class CreatorBuilder : ICreatorBuilder
{
    private Faker<Creator> _faker = null!;

    public CreatorBuilder() => Reset();

    public void Reset() => _faker = new Faker<Creator>()
            .UseSeed(61146)
            .StrictMode(true);

    public Creator GetCreator()
    {
        Creator result = _faker.Generate();
        Reset();
        return result;
    }

    public ICreatorBuilder WithId(Guid id)
    {
        _faker.RuleFor(c => c.Id, id);
        return this;
    }

    public ICreatorBuilder WithName(string name)
    {
        _faker.RuleFor(c => c.Name, name);
        return this;
    }

    public ICreatorBuilder WithOtherNames(HashSet<OtherName> otherNames)
    {
        _faker.RuleFor(c => c.OtherNames, (_, c) =>
                {
                    foreach (var otherName in otherNames)
                        c.OtherNames.Add(otherName);
                    return c.OtherNames;
                });
        return this;
    }

    public ICreatorBuilder WithSex(Sex sex)
    {
        _faker.RuleFor(c => c.Sex, sex);
        return this;
    }
}
