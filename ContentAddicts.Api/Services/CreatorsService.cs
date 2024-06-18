using ContentAddicts.Api.Models;

namespace ContentAddicts.Api.Services;

public interface ICreatorsService
{
    public Task<IEnumerable<Creator>> GetCreators();
}

public class CreatorsService : ICreatorsService
{
    public Task<IEnumerable<Creator>> GetCreators()
    {
        throw new NotImplementedException();
    }
}
