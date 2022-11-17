using System.Text;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ContainerRegistry.Controllers;

[Route("[controller]")]
public class ConnectController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConnectService _connectService;

    public ConnectController(IUserService userService, IConnectService connectService)
    {
        _userService = userService;
        _connectService = connectService;
    }

    [HttpGet("token")]
    public async Task<IActionResult> Token(CancellationToken cancellationToken)
    {
        var authorization = Request.Headers[HeaderNames.Authorization].ToString();
        var account = Request.Query["account"].ToString();
        var scope = Request.Query["scope"].ToString();
        
        if (!string.IsNullOrEmpty(scope))
        {
            
        }
        
        
        
        if (authorization != null && authorization.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            var basic = authorization["Basic ".Length..].Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(basic)).Split(':');
            var userName = credentials[0];
            var password = credentials[1];
            if (!await _userService.ValidateAsync(userName,password))
            {
                return Unauthorized("invalid auth credentials");
            }
            
            var token = await _connectService.CreateTokenAsync(cancellationToken);
            return Ok(token);
        }
        
        return Unauthorized("invalid auth credentials");
    }
}