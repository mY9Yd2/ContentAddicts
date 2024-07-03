using System.Text.Json.Serialization;

namespace ContentAddicts.Api.Models;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/ISO/IEC_5218">ISO/IEC 5218</see>
/// Information technology — Codes for the representation of human sexes
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<Sex>))]
public enum Sex
{
    NotKnown = 0,
    Male = 1,
    Female = 2,
    NotApplicable = 9
}
