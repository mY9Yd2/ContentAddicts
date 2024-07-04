using Bogus;

using ContentAddicts.Api.Models;
using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Directors;

public static class GetCreatorDtoDirector
{
    private readonly static Faker F = new() { Random = new(17205) };
    private readonly static OtherNameBuilder OtherNameBuilder = new();

    public static T BuildRandomGetCreatorDto<T>(this IGetCreatorDtoBuilder builder) where T : IGetCreatorDtoBuilder
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
