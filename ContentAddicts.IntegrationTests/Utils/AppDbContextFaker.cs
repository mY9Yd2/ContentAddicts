using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.IntegrationTests.Utils;

public class AppDbContextFaker
{
    public Faker<GetCreatorDto> GetCreatorFaker { get; } = new Faker<GetCreatorDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid());
    public Faker<GetAllCreatorsDto> GetAllCreatorsFaker { get; } = new Faker<GetAllCreatorsDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid());
    public Faker<CreateCreatorDto> CreateCreatorFaker { get; } = new Faker<CreateCreatorDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid());
    private static readonly int Seed = 8;
}
