using ContentAddicts.Api.Contexts;

using ErrorOr;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace ContentAddicts.Api.UseCases.Creators.Delete;

public class DeleteCreatorHandler(AppDbContext context) : IRequestHandler<DeleteCreatorCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteCreatorCommand request, CancellationToken cancellationToken)
    {
        bool isCreatorExists = await context.Creators.AnyAsync(c => c.Id == request.CreatorId, cancellationToken);

        if (!isCreatorExists)
        {
            return Error.NotFound(description: "A creator with this id does not exist!");
        }

        await context.Creators
                .Where(c => c.Id == request.CreatorId)
                .ExecuteDeleteAsync(cancellationToken);

        return Result.Deleted;
    }
}
