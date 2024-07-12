using FluentValidation;

namespace ContentAddicts.Api.Extensions;

public static class ValidatorExtension
{
    public static IRuleBuilderOptions<T, string> NoLeadingOrTrailingWhitespace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
                .NoLeadingWhitespace()
                .NoTrailingWhitespace();
    }

    public static IRuleBuilderOptions<T, string> NoLeadingWhitespace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
                .Must(str => str is null || str.Length == 0 || !char.IsWhiteSpace(str.First()))
                .WithMessage("'{PropertyName}' must not start with a whitespace character.");
    }

    public static IRuleBuilderOptions<T, string> NoTrailingWhitespace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
                .Must(str => str is null || str.Length == 0 || !char.IsWhiteSpace(str.Last()))
                .WithMessage("'{PropertyName}' must not end with a whitespace character.");
    }
}
