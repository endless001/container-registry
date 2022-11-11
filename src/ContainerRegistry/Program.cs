using ContainerRegistry;
using ContainerRegistry.Aliyun;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Database.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddContainerRegistry()
    .AddAliyunOssStorage()
    .AddPostgreSqlDatabase();

var gitHubOptions = builder.Configuration.GetSection("Authentications:GitHub").Get<GitHubAuthenticationOptions>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
}).AddGitHub(options =>
{
    options.ClientId = gitHubOptions.ClientId;
    options.ClientSecret = gitHubOptions.ClientSecret;
    options.CallbackPath = gitHubOptions.CallbackPath;
    options.SaveTokens = true;
});

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();