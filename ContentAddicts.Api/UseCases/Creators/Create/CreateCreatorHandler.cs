using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Models;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.Create;

public class CreateCreatorHandler(AppDbContext context) : IRequestHandler<CreateCreatorCommand, ErrorOr<GetCreatorDto>>
{
    public async Task<ErrorOr<GetCreatorDto>> Handle(CreateCreatorCommand command, CancellationToken cancellationToken)
    {
        if (await context.Creators
                .AnyAsync(c => c.Id == command.Id, cancellationToken))
        {
            return Error.Conflict(description: "A creator with this id already exists!");
        }

        var creator = new Creator()
        {
            Id = command.Id
        };

        await context.Creators.AddAsync(creator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new GetCreatorDto()
        {
            Id = creator.Id
        };
    }
}
