using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Builders;

public class OtherNameBuilder : IOtherNameBuilder
{
    private Faker<OtherName> _faker = null!;

    public OtherNameBuilder() => Reset();

    public void Reset() => _faker = new Faker<OtherName>()
            .UseSeed(52587)
            .StrictMode(true)
                    .Ignore(o => o.Id)
                    .Ignore(o => o.CreatorId)
                    .Ignore(o => o.Creator);

    public OtherName GetOtherName()
    {
        OtherName result = _faker.Generate();
        Reset();
        return result;
    }

    public IOtherNameBuilder WithName(string name)
    {
        _faker.RuleFor(c => c.Name, name);
        return this;
    }
}
