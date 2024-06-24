using ContentAddicts.Api.Models;

namespace ContentAddicts.UnitTests.Fixtures;

public class CreatorsFixture
{
    private readonly Faker<Creator> _faker;

    public List<Creator> CreatorsTestData { get; private set; }

    public CreatorsFixture()
    {
        _faker = new Faker<Creator>()
                .UseSeed(8)
                .RuleFor(o => o.Id, f => f.Random.Guid());

        CreatorsTestData = _faker.Generate(4);
    }
}
