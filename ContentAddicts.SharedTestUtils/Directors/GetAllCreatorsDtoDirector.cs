using Bogus;

using ContentAddicts.SharedTestUtils.Builders;
using ContentAddicts.SharedTestUtils.Interfaces;

namespace ContentAddicts.SharedTestUtils.Directors;

public static class GetAllCreatorsDtoDirector
{
    private readonly static Faker F = new() { Random = new(58756) };
    private readonly static OtherNameBuilder OtherNameBuilder = new();

    public static T BuildRandomGetAllCreatorsDto<T>(this IGetAllCreatorsDtoBuilder builder) where T : IGetAllCreatorsDtoBuilder
    {
        return (T)builder
                .WithId(F.Random.Guid())
                .WithName(F.Internet.UserName())
                .WithOtherNames(
                        [
                            OtherNameBuilder.BuildRandomOtherName<OtherNameBuilder>()
                                    .GetOtherName().Name,
                            OtherNameBuilder.BuildRandomOtherNameUnicode<OtherNameBuilder>()
                                    .GetOtherName().Name
                        ]);
    }
}
