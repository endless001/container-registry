using ContainerRegistry.Core.Entities;
using ContainerRegistry.Protocol.GitHub;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Services;

public class UserService : IUserService
{
    private readonly IContext _context;
    private readonly GitHubClient _gitHubClient;

    public UserService(IContext context, GitHubClient gitHubClient)
    {
        _context = context;
        _gitHubClient = gitHubClient;
    }

    public ValueTask<User> FindAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Users.FindAsync(id);
    }

    public async ValueTask<bool> ValidateAsync(string userName, string secret)
    {
        return await _context.Users.Where(u => u.UserName == userName && u.Secret == secret).AnyAsync();
    }

    public async ValueTask SynchronizeUserAsync(string accessToken)
    {
        var userResponse = await _gitHubClient.GetUserAsync();
        var user = await _context.Users.Where(u => u.UserName == userResponse.Login).FirstOrDefaultAsync();
        if (user is null)
        {
            user = new User
            {
                Token = accessToken,
                UserName = userResponse.Login,
                Email = userResponse.Email,
                Avatar = userResponse.Avatar,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };
            _context.Users.Add(user);
        }
        else
        {
            user.Token = accessToken;
            user.UserName = userResponse.Login;
            user.Email = userResponse.Email;
            user.Avatar = userResponse.Avatar;
            user.Updated = DateTime.UtcNow;
            
            _context.Users.Update(user);
        }

        await _context.SaveChangesAsync(new CancellationToken());
    }
}