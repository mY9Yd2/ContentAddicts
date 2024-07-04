using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Directors;

public static class CreateCreatorDtoDirector
{
    private readonly static Faker F = new() { Random = new(63313) };
    private readonly static OtherNameBuilder OtherNameBuilder = new();

    public static T BuildRandomCreateCreatorDto<T>(this ICreateCreatorDtoBuilder builder) where T : ICreateCreatorDtoBuilder
    {
        return (T)builder
                .WithId(F.Random.Guid())
                .WithName(F.Internet.UserName())
                .WithOtherNames(
                        [
                            OtherNameBuilder.BuildRandomOtherName<OtherNameBuilder>()
                                    .Build().Name,
                            OtherNameBuilder.BuildRandomOtherNameUnicode<OtherNameBuilder>()
                                    .Build().Name
                        ])
                .WithSex(F.PickRandom<Sex>());
    }
}
