using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;
 
[Route("[controller]")]
[ApiController]
public class OAuthController : ControllerBase
{ 
    [HttpGet("signin")]
    public IActionResult SignIn(string provider)
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/oauth/callback" }, provider);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        Console.WriteLine(1);
        return Ok();
    }

}