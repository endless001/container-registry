using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IRepositoryService
{
    Task<PagedList<RepositoryResponse>> GetAsync(int organizationId, int pageSize, int pageIndex);
    Task<RepositoryResponse> GetAsync(int organizationId, string repository);
    ValueTask<bool> AllowAccessAsync(string account, Scope scope);
    ValueTask AddDownloadAsync();
}