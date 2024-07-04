using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.Api.UseCases.Creators;

namespace ContentAddicts.SharedTestUtils.Fakers;

public class AppDbContextFaker
{
    public Faker<GetCreatorDto> GetCreatorFaker { get; } = new Faker<GetCreatorDto>()
            .UseSeed(Seed)
            .StrictMode(true)
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Person.UserName)
            .RuleFor(c => c.OtherNames, f => [
                            f.Internet.UserNameUnicode(),
                            f.Internet.UserName()])
            .RuleFor(c => c.Sex, f => f.PickRandom<Sex>());
    public Faker<GetAllCreatorsDto> GetAllCreatorsFaker { get; } = new Faker<GetAllCreatorsDto>()
            .UseSeed(Seed)
            .StrictMode(true)
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Person.UserName)
            .RuleFor(c => c.OtherNames, f => [
                            f.Internet.UserNameUnicode(),
                            f.Internet.UserName()]);
    public Faker<CreateCreatorDto> CreateCreatorFaker { get; } = new Faker<CreateCreatorDto>()
            .UseSeed(Seed)
            .StrictMode(true)
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Person.UserName)
            .RuleFor(c => c.OtherNames, f => [
                            f.Internet.UserNameUnicode(),
                            f.Internet.UserName()])
            .RuleFor(c => c.Sex, f => f.PickRandom<Sex>());

    private static readonly int Seed = 8;
}
