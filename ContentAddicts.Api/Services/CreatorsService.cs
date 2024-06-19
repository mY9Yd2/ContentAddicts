using ContentAddicts.Api.Models;

namespace ContentAddicts.Api.Services;

public interface ICreatorsService
{
    public Task<IEnumerable<Creator>> GetCreators();
    public Task<Creator?> GetCreator(Guid id);
}

public class CreatorsService : ICreatorsService
{
    public Task<IEnumerable<Creator>> GetCreators()
    {
        throw new NotImplementedException();
    }

    public Task<Creator?> GetCreator(Guid id)
    {
        throw new NotImplementedException();
    }
}
