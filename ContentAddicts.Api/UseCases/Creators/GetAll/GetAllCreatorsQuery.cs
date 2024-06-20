using ContentAddicts.Api.Models;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.GetAll;

public record GetAllCreatorsQuery : IRequest<List<Creator>>;
