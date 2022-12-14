using AspNet.Security.OAuth.GitHub;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[ApiController]
public class OidcController : ControllerBase
{
    private readonly IUserService _userService;

    public OidcController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = Url.Action("Synchronize") },
            GitHubAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("synchronize")]
    [Authorize]
    public async Task<IActionResult> Synchronize()
    {
        const string ACCESS_TOKEN = "access_token";

        var accessToken = await HttpContext.GetTokenAsync(ACCESS_TOKEN);
        await _userService.SynchronizeUserAsync(accessToken);
        return Redirect("/");
    }
}