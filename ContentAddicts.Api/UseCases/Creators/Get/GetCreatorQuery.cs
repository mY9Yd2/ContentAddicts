using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Get;

public record GetCreatorQuery(Guid CreatorId) : IRequest<GetCreatorDto?>;
