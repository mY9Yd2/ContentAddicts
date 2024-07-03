using ContentAddicts.Api.Models;

namespace ContentAddicts.Api.UseCases.Creators;

public record CreateCreatorDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public HashSet<string> OtherNames { get; init; } = [];
    public required Sex Sex { get; init; } = Sex.NotKnown;
};
