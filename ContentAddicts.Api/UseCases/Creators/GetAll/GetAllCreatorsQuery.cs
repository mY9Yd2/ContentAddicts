using ErrorOr;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.GetAll;

public record GetAllCreatorsQuery : IRequest<ErrorOr<List<GetAllCreatorsDto>>>;
