using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Directors;

public static class CreatorDirector
{
    private readonly static Faker F = new() { Random = new(43163) };
    private readonly static OtherNameBuilder OtherNameBuilder = new();

    public static T BuildRandomCreator<T>(this ICreatorBuilder builder) where T : ICreatorBuilder
    {
        return (T)builder
                .WithId(F.Random.Guid())
                .WithName(F.Internet.UserName())
                .WithOtherNames(
                        [
                            OtherNameBuilder.BuildRandomOtherName<OtherNameBuilder>()
                                    .Build(),
                            OtherNameBuilder.BuildRandomOtherNameUnicode<OtherNameBuilder>()
                                    .Build()
                        ])
                .WithSex(F.PickRandom<Sex>());
    }
}
