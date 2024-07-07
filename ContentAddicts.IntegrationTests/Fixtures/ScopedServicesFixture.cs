using Microsoft.Extensions.DependencyInjection;

namespace ContentAddicts.IntegrationTests.Fixtures;

public class ScopedServicesFixture : IDisposable
{
    public required IServiceScope Scope { private get; set; }
    public IServiceProvider ServiceProvider { get => Scope.ServiceProvider; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Scope.Dispose();
    }
}
