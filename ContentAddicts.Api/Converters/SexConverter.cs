using ContentAddicts.Api.Models;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContentAddicts.Api.Converters;

public class SexConverter : ValueConverter<Sex, string>
{
    public SexConverter()
        : base(
            v => v.ToString(),
            v => (Sex)Enum.Parse(typeof(Sex), v, true))
    { }
}
