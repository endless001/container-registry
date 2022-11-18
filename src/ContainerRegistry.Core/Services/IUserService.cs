using ContainerRegistry.Core.Entities;

namespace ContainerRegistry.Core.Services;

public interface IUserService
{
    ValueTask<User> FindAsync(int id, CancellationToken cancellationToken);
    ValueTask<bool> ValidateAsync(string userName, string secret);
    ValueTask SynchronizeUserAsync(string accessToken);
}