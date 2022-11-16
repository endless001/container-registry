using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContainerRegistry.Filter;

public class AccessActionFilter : IAsyncActionFilter
{

    private readonly string _endpoint;
    private readonly string _service;

    public AccessActionFilter()
    {
        _endpoint = "http://apps.io:5000";
        _service = "registry.zero.com";
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ctx = context.HttpContext;
        var challenge = $"""Bearer realm="{ _endpoint}/connect/token",service="{ _service}" """ ;

        var loginPath = "/v2/";
        var targetPath = ctx.Request.Path;
        if (loginPath == targetPath && !ctx.User.Identity.IsAuthenticated)
        {
            ctx.Response.Headers.Add("WWW-Authenticate", challenge);
            ctx.Response.Headers.Add("Docker-Distribution-API-Version", "registry/2.0");
            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        await next();
    }
}