var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("v2", (HttpContext context) => Results.Unauthorized());

app.MapPost("/v2/{name}/blobs/", (string name, HttpContext context) =>
{
    Console.WriteLine(1);
});

app.Map("/v2/{name}/blobs/{digest}", (string name, string digest, HttpContext context) =>
{
    context.Response.Headers.Add("Docker-Content-Digest", digest);
    Results.NotFound();
});

app.MapPut(" /v2/{name}/blobs/uploads/{uuid}", (string name,string uuid) =>
{
    Console.WriteLine($"{name}/{uuid}");
});

app.Run();