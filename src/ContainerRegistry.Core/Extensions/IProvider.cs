using Microsoft.Extensions.Configuration;

namespace ContainerRegistry.Core.Extensions;

public interface IProvider<TService>
{
    TService GetOrNull(IServiceProvider provider, IConfiguration configuration);
}

internal class DelegateProvider<TService> : IProvider<TService>
{
    private readonly Func<IServiceProvider, IConfiguration, TService> _func;

    public DelegateProvider(Func<IServiceProvider, IConfiguration, TService> func)
    {
        _func = func ?? throw new ArgumentNullException(nameof(func));
    }

    public TService GetOrNull(IServiceProvider provider, IConfiguration configuration)
    {
        return _func(provider, configuration);
    }
}