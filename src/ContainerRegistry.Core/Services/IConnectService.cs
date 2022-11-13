namespace ContainerRegistry.Core.Services;

public interface IConnectService
{
    Task CreateTokenAsync(CancellationToken cancellationToken);
}