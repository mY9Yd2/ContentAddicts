using FluentValidation;

namespace ContentAddicts.Api.UseCases.Creators;

public class CreateCreatorDtoValidator : AbstractValidator<CreateCreatorDto>
{
    public CreateCreatorDtoValidator()
    {
        RuleFor(c => c.Id)
                .NotEmpty();
        RuleFor(c => c.Name)
                .NotEmpty()
                .Length(1, 32);
        RuleFor(c => c.OtherNames)
                .Must(o => o.Count <= 25);
        RuleForEach(c => c.OtherNames)
                .NotEmpty()
                .Length(1, 32)
                .NotEqual(c => c.Name);
        RuleFor(c => c.Sex)
                .IsInEnum();
    }
}
