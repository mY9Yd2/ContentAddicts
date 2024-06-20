using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.GetAll;

public class GetAllCreatorsHandler(AppDbContext context) : IRequestHandler<GetAllCreatorsQuery, List<Creator>>
{
    public async Task<List<Creator>> Handle(GetAllCreatorsQuery request, CancellationToken cancellationToken)
    {
        return await context.Creators.ToListAsync(cancellationToken);
    }
}
