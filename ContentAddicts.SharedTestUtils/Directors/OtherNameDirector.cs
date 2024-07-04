using Bogus;

using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Directors;

public static class OtherNameDirector
{
    private readonly static Faker F = new() { Random = new(50089) };

    public static T BuildRandomOtherName<T>(this IOtherNameBuilder builder) where T : IOtherNameBuilder
    {
        return (T)builder
                .WithName(F.Internet.UserNameUnicode());
    }
}
