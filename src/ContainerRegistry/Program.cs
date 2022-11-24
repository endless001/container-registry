using System.Text;
using ContainerRegistry.Aliyun;
using Microsoft.IdentityModel.Tokens;
using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Database.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddContainerRegistry()
    .AddAliyunOssStorage()
    .AddPostgreSqlDatabase();

var gitHubOptions = builder.Configuration.GetSection("Authentications:GitHub").Get<GitHubAuthenticationOptions>();
var jwtBearerOptions = builder.Configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/login";
}).AddGitHub(options =>
{
    options.ClientId = gitHubOptions.ClientId;
    options.ClientSecret = gitHubOptions.ClientSecret;
    options.CallbackPath = gitHubOptions.CallbackPath;
    options.SaveTokens = true;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidIssuer = jwtBearerOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerOptions.SignKey))
    };
    var challenge = string.Format("""Bearer realm="{Endpoint}",service="{Service}" """,
        builder.Configuration["Endpoint"],
        builder.Configuration["Service"]);
    options.Challenge = challenge;
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