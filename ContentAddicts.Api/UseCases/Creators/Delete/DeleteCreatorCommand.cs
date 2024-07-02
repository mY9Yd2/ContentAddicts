using ErrorOr;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Delete;

public record DeleteCreatorCommand(Guid CreatorId) : IRequest<ErrorOr<Deleted>>;
