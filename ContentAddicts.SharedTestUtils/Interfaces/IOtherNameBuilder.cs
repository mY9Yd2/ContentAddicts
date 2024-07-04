using ContentAddicts.Api.Models;

namespace ContentAddicts.SharedTestUtils.Interfaces;

public interface IOtherNameBuilder : IBuilderBase<OtherName>
{
    IOtherNameBuilder WithName(string name);
}
