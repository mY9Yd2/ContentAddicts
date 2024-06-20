using ContentAddicts.Api.Models;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Get;

public class GetCreatorHandler() : IRequestHandler<GetCreatorQuery, Creator?>
{
    public Task<Creator?> Handle(GetCreatorQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
