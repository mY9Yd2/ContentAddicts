using ContentAddicts.Api.Models;

namespace ContentAddicts.UnitTests.Utils;

public class AppDbContextFaker
{
    public Faker<Creator> CreatorFaker { get; private init; }

    public AppDbContextFaker()
    {
        CreatorFaker = GetCreatorFaker();
    }

    private static Faker<Creator> GetCreatorFaker()
    {
        return new Faker<Creator>()
                .UseSeed(8)
                .RuleFor(o => o.Id, f => f.Random.Guid());
    }
}
