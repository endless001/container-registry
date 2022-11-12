using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IOrganizationService
{
    Task<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken);
    Task<PaginatedItems<Organization>> GetAsync(string @namespace, int pageSize, int pageIndex);
}