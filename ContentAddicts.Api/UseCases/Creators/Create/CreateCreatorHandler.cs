using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Models;

using MediatR;

namespace ContentAddicts.Api.UseCases.Creators.Create;

public class CreateCreatorHandler(AppDbContext context) : IRequestHandler<CreateCreatorCommand, GetCreatorDto>
{
    public async Task<GetCreatorDto> Handle(CreateCreatorCommand command, CancellationToken cancellationToken)
    {
        var creator = new Creator()
        {
            Id = command.Id
        };

        await context.Creators.AddAsync(creator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return creator.ToGetCreatorDto();
    }
}
