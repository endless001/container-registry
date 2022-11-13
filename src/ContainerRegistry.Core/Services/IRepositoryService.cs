using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IRepositoryService
{
    Task<PaginatedItems<Repository>> GetAsync(int organizationId, int pageSize, int pageIndex);
}