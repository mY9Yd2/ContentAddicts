using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Fakers;

public class AppDbContextFaker
{
    public Faker<GetCreatorDto> GetCreatorFaker { get; } = new Faker<GetCreatorDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.Name, f => f.Person.UserName);
    public Faker<GetAllCreatorsDto> GetAllCreatorsFaker { get; } = new Faker<GetAllCreatorsDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.Name, f => f.Person.UserName);
    public Faker<CreateCreatorDto> CreateCreatorFaker { get; } = new Faker<CreateCreatorDto>()
            .UseSeed(Seed)
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.Name, f => f.Person.UserName);
    public Faker<OtherName> OtherNameFaker { get; } = new Faker<OtherName>()
            .UseSeed(Seed)
            .RuleFor(o => o.Name, f => f.Person.UserName);
    private static readonly int Seed = 8;
}
