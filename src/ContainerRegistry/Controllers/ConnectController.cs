using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("[controller]")]
public class ConnectController : ControllerBase
{
    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        return Ok();
    }
}

