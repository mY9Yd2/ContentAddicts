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
    public Faker<OtherName> OtherNameFaker { get; } = new Faker<OtherName>()
            .UseSeed(Seed)
            .StrictMode(false)
            .RuleFor(o => o.Name, f => f.Person.UserName);
    public Faker<Creator> CreatorFaker { get; } = new Faker<Creator>()
            .UseSeed(Seed)
            .StrictMode(true)
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Internet.UserName())
            .RuleFor(c => c.OtherNames, f => [
                            new() { Name = f.Internet.UserNameUnicode() },
                            new() { Name = f.Internet.UserName() }])
            .RuleFor(c => c.Sex, f => f.PickRandom<Sex>());
    private static readonly int Seed = 8;
}
