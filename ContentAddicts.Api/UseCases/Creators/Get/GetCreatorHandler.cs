using ContentAddicts.Api.Contexts;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.Get;

public class GetCreatorHandler(AppDbContext context) : IRequestHandler<GetCreatorQuery, GetCreatorDto?>
{
    public async Task<GetCreatorDto?> Handle(GetCreatorQuery request, CancellationToken cancellationToken)
    {
        return await context.Creators.Select(c => new GetCreatorDto()
        {
            Id = c.Id
        }).FirstOrDefaultAsync(cancellationToken);
    }
}
