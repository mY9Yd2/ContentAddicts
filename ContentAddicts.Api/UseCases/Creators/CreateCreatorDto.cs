namespace ContentAddicts.Api.UseCases.Creators;

public record CreateCreatorDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public HashSet<string> OtherNames { get; init; } = [];
};
