namespace ContentAddicts.Api.UseCases.Creators;

public record GetAllCreatorsDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public ICollection<string> OtherNames { get; init; } = [];
}
