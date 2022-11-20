using System.Text;
using ContainerRegistry.Core.Models;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ContainerRegistry.Controllers;

[Route("[controller]")]
public class ConnectController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConnectService _connectService;
    private readonly IRepositoryService _repositoryService;

    public ConnectController(IUserService userService, IConnectService connectService,
        IRepositoryService repositoryService)
    {
        _userService = userService;
        _connectService = connectService;
        _repositoryService = repositoryService;
    }

    [HttpGet("token")]
    public async Task<IActionResult> Token(CancellationToken cancellationToken)
    {
        var authorization = Request.Headers[HeaderNames.Authorization].ToString();
        var account = Request.Query["account"].ToString();
        var scopeValues = Request.Query["scope"].ToString();
        bool.TryParse(Request.Query["offline_token"].ToString(), out var offlineToken);

        if (offlineToken && authorization != null &&
            authorization.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            var basic = authorization["Basic ".Length..].Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(basic)).Split(':');
            var userName = credentials[0];
            var password = credentials[1];
            if (!await _userService.ValidateAsync(userName, password))
            {
                return Unauthorized("invalid auth credentials");
            }

            var token = await _connectService.CreateTokenAsync(cancellationToken);
            return Ok(token);
        }

        if (string.IsNullOrEmpty(scopeValues)) return Unauthorized("invalid auth credentials");
        {
            var scopes = scopeValues.Split(":");
            var scope = new Scope
            {
                Type = scopes[0],
                RepositoryName = scopes[1],
                Action = scopes[2]
            };
            var allowAccess = await _repositoryService.AllowAccessAsync(account, scope);
            if (!allowAccess)
            {
                return Unauthorized("invalid auth credentials");
            }

            var token = await _connectService.CreateTokenAsync(cancellationToken);
            return Ok(token);
        }
    }
}