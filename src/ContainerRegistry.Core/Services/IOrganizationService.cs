namespace ContainerRegistry.Core.Services;

public interface IOrganizationService
{
    Task<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken);
}