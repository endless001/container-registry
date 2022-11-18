using ContainerRegistry.Core.Entities;
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

    public async Task<PaginatedItems<Repository>> GetAsync(int organizationId, int pageSize, int pageIndex)
    {
        var totalItems = await _context.Repositories
            .LongCountAsync();

        var itemsOnPage = await _context.Repositories
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        var model = new PaginatedItems<Repository>(pageIndex, pageSize, totalItems, itemsOnPage);
        return model;
    }

    public async ValueTask<bool> AllowAccessAsync(string account, Scope scope)
    {
        var repository = await _context.Repositories
            .Include(x => x.Organization)
            .Include(x => x.Accesses).FirstOrDefaultAsync(r =>
                r.Name == scope.RepositoryName && r.Organization.Namespace == scope.Namespace);

        if (repository.Visible == 1 && scope.Action.Equals("pull", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return repository.Accesses.Any(x => x.MemberId == 1 && x.Action == 1);
    }
}