using ContentAddicts.Api.Contexts;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.Get;

public class GetCreatorHandler(AppDbContext context) : IRequestHandler<GetCreatorQuery, ErrorOr<GetCreatorDto>>
{
    public async Task<ErrorOr<GetCreatorDto>> Handle(GetCreatorQuery request, CancellationToken cancellationToken)
    {
        var creator = await context.Creators
                .Select(c => new GetCreatorDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    OtherNames = c.OtherNames.Select(o => o.Name).ToList(),
                    Sex = c.Sex
                })
                .FirstOrDefaultAsync(c => c.Id == request.CreatorId, cancellationToken);

        if (creator is null)
        {
            return Error.NotFound(description: "A creator with this id does not exist!");
        }

        return creator;
    }
}
