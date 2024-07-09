using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

using FluentValidation.Internal;

namespace ContentAddicts.Api.Policies;

using Resolver = Func<Type, MemberInfo, LambdaExpression, string>;

public static class ValidatorDisplayNamePolicy
{
    // Both are quite similar, so I think it is fine to call the Microsoft version of the method, rather than copy and paste the code.
    // https://github.com/FluentValidation/FluentValidation/issues/226#issuecomment-197893354
    // https://github.com/dotnet/runtime/blob/5b5d7919a6099c9ea0056eaa800d59f53b3d9fe1/src/libraries/System.Text.Json/Common/JsonCamelCaseNamingPolicy.cs

    public static Resolver CamelCase { get; } =
            (_, memberInfo, expression) => Resolve(memberInfo, expression, JsonNamingPolicy.CamelCase.ConvertName);

    private static string Resolve(MemberInfo memberInfo, LambdaExpression expression, Func<string, string> convertName)
    {
        if (expression is not null)
        {
            PropertyChain chain = PropertyChain.FromExpression(expression);
            if (chain.Count > 0) return convertName(chain.ToString());
        }

        return convertName(memberInfo?.Name ?? string.Empty);
    }
}
