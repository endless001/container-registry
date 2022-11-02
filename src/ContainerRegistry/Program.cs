using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("v2", (HttpContext context) => Results.Unauthorized());

app.Map("/v2/{name}/blobs/{digest}", (string name, string digest, HttpContext context) =>
{
    var hash = digest.Split(":").Last();
    if (File.Exists($"/{hash}"))
    {
        context.Response.Headers.Add("content-length", new FileInfo(hash).Length.ToString());
        context.Response.Headers.Add("docker-content-digest", digest);
        Results.Ok();
    }

    Results.NotFound();
});

app.MapPost("/v2/{name}/blobs/uploads", (string name, HttpContext context) =>
{
    var uuid = Guid.NewGuid().ToString();
    context.Response.Headers.Add("location", $"/v2/{name}/blobs/uploads/{uuid}");
    context.Response.Headers.Add("range", "0-0");
    context.Response.Headers.Add("content-length", "0");
    context.Response.Headers.Add("docker-upload-uuid", uuid);
    Results.Accepted();
});

app.MapPatch(" /v2/{name}/blobs/uploads/{uuid}",
   async (string name, string uuid, [FromHeader] string range, HttpContext context) =>
    {
        var start = range.Split("-")[0];
        await using var fs = File.OpenWrite(uuid);
        fs.Seek(long.Parse(start), SeekOrigin.Begin);
        await context.Request.Body.CopyToAsync(fs);
        context.Response.Headers.Add("range", $"0-{fs.Position - 1}");
        
        context.Response.Headers.Add("docker-upload-uuid",uuid);
        context.Response.Headers.Add("location", $"/v2/{name}/blobs/uploads/{uuid}");
        context.Response.Headers.Add("content-length","0");
        context.Response.Headers.Add("docker-distribution-api-version","registry/2.0");
        Results.Accepted();
    });

app.MapPut(" /v2/{name}/blobs/uploads/{uuid}", async (string name, string uuid, HttpContext context) =>
{
    var length = context.Request.Headers["content-length"].ToString();
    if (length!="0")
    {
        var ranges = context.Request.Headers["content-range"].ToString().Split("-");
        await using var fs = File.OpenWrite(uuid);
        fs.Seek(long.Parse(ranges[0]), SeekOrigin.Begin);
        await context.Request.Body.CopyToAsync(fs);
    }

    var rawDigest = context.Request.Query["digest"].ToString();
    var digest = context.Request.Query["digest"].ToString().Split(":").Last();
    File.Move(uuid, digest);
    context.Response.Headers.Add("content-length", "0");
    context.Response.Headers.Add("docker-content-digest", rawDigest);
    Results.Created($"/v2/{name}/blobs/{digest}", string.Empty);
});


app.Run();