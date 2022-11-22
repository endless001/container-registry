using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Services;

public interface IOrganizationService
{
    ValueTask<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken);
    Task<OrganizationResponse> GetAsync(string @namespace, CancellationToken cancellationToken);
    Task<PagedList<OrganizationResponse>> GetAsync(string @namespace, int pageSize, int pageIndex);
    ValueTask<bool> CreateAsync(Organization organization, CancellationToken cancellationToken);
    ValueTask<bool> UpdateAsync(Organization organization, CancellationToken cancellationToken);
    Task<List<MemberResponse>> GetMemberAsync(int id, CancellationToken cancellationToken);
}