using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IConnectService
{
    Task<Tokens> CreateTokenAsync(CancellationToken cancellationToken);
}