using FluentValidation;

namespace ContentAddicts.Api.UseCases.Creators.Create;

public class CreateCreatorCommandValidator : AbstractValidator<CreateCreatorCommand>
{
    public CreateCreatorCommandValidator()
    {
        Include(new CreateCreatorDtoValidator());
    }
}
