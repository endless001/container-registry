using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IRepositoryService
{
    Task<PagedList<Repository>> GetAsync(int organizationId, int pageSize, int pageIndex);
    ValueTask<bool> AllowAccessAsync(string account, Scope scope);
    ValueTask AddDownloadAsync();
}