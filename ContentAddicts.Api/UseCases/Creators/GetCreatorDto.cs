using ContentAddicts.Api.Models;

namespace ContentAddicts.Api.UseCases.Creators;

public record GetCreatorDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public ICollection<string> OtherNames { get; init; } = [];
    public required Sex Sex { get; init; }
};
