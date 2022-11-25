using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Enums;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Core.Mappers;
using ContainerRegistry.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Services;

public class RepositoryService : IRepositoryService
{
    private readonly IContext _context;

    public RepositoryService(IContext context)
    {
        _context = context;
    }

    public async Task<PagedList<RepositoryResponse>> GetAsync(string @namespace, int pageSize, int pageIndex)
    {
        var repositories = await _context.Repositories
            .Include(x => x.Organization)
            .Where(x => x.Organization.Namespace == @namespace)
            .PageBy(x => x.Id, pageIndex, pageSize).ToListAsync();
        var totalCount = await _context.Repositories.LongCountAsync();

        var result = new PagedList<RepositoryResponse>(pageIndex, pageSize, totalCount,
            repositories.ToRepositoryModel<List<RepositoryResponse>>());
        return result;
    }

    public async Task<RepositoryResponse> GetAsync(string @namespace, string name)
    {
        var repository = await _context.Repositories
            .Include(x => x.Organization)
            .Where(x => x.Organization.Namespace == @namespace && x.Name == name).FirstOrDefaultAsync();
        
        return repository.ToRepositoryModel<RepositoryResponse>();
    }

    public async ValueTask<bool> AllowAccessAsync(string account, Scope scope)
    {
        var repository = await _context.Repositories
            .Include(x => x.Organization)
            .Include(x => x.Accesses).FirstOrDefaultAsync(r =>
                r.Name == scope.RepositoryName && r.Organization.Namespace == scope.Namespace);

        if (repository.Visible == (int)VisibleType.Publish && scope.Action == ActionType.Pull)
        {
            return true;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == account);
        return repository.Accesses.Where(x => x.MemberId == user?.Id)
            .Any(x => x.Action == (int)scope.Action || x.Action == (int)ActionType.All);
    }

    public ValueTask AddDownloadAsync()
    {
        throw new NotImplementedException();
    }
}