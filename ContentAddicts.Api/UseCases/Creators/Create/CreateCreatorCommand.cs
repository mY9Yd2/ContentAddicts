using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Create;

public record CreateCreatorCommand : CreateCreatorDto, IRequest<GetCreatorDto>;
