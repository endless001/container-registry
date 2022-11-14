using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IConnectService
{
    Task<Token> CreateTokenAsync(CancellationToken cancellationToken);
}