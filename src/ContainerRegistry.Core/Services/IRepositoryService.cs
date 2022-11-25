using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IRepositoryService
{
    Task<PagedList<RepositoryResponse>> GetAsync(string @namespace, int pageSize, int pageIndex);
    Task<RepositoryResponse> GetAsync(string @namespace, string name);
    ValueTask<bool> AllowAccessAsync(string account, Scope scope);
    ValueTask AddDownloadAsync();
}