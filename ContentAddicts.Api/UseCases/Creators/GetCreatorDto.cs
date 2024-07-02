namespace ContentAddicts.Api.UseCases.Creators;

public record GetCreatorDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public ICollection<string> OtherNames { get; init; } = [];
};
