using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Models;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Get;

public class GetCreatorHandler(AppDbContext context) : IRequestHandler<GetCreatorQuery, Creator?>
{
    public async Task<Creator?> Handle(GetCreatorQuery request, CancellationToken cancellationToken)
    {
        return await context.Creators.FindAsync([request.CreatorId], cancellationToken);
    }
}
