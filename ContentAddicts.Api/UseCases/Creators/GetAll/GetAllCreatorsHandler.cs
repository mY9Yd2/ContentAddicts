using ContentAddicts.Api.Contexts;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.GetAll;

public class GetAllCreatorsHandler(AppDbContext context) : IRequestHandler<GetAllCreatorsQuery, ErrorOr<List<GetAllCreatorsDto>>>
{
    public async Task<ErrorOr<List<GetAllCreatorsDto>>> Handle(GetAllCreatorsQuery request, CancellationToken cancellationToken)
    {
        return await context.Creators.Select(c => new GetAllCreatorsDto()
        {
            Id = c.Id
        }).ToListAsync(cancellationToken);
    }
}
