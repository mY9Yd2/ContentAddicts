using ContentAddicts.Api.Models;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.GetAll;

public class GetAllCreatorsHandler() : IRequestHandler<GetAllCreatorsQuery, List<Creator>>
{
    public Task<List<Creator>> Handle(GetAllCreatorsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
