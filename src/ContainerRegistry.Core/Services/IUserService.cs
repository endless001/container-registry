using ContainerRegistry.Core.Entities;

namespace ContainerRegistry.Core.Services;

public interface IUserService
{
    Task<User> FindAsync(int id, CancellationToken cancellationToken);
    Task SynchronizeUserAsync(string accessToken);
}