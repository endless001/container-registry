using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IOrganizationService
{
    ValueTask<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken);
    Task<PaginatedItems<Organization>> GetAsync(string @namespace, int pageSize, int pageIndex);
    ValueTask<bool> CreateAsync(Organization organization, CancellationToken cancellationToken);
    ValueTask<bool> UpdateAsync(Organization organization, CancellationToken cancellationToken);
}