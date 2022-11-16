using System.Text;
using ContainerRegistry;
using ContainerRegistry.Aliyun;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Database.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddContainerRegistry()
    .AddAliyunOssStorage()
    .AddPostgreSqlDatabase(); 


var gitHubOptions = builder.Configuration.GetSection("Authentications:GitHub").Get<GitHubAuthenticationOptions>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
}).AddMicrosoftAccount(options =>
{
    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
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
        ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:SignKey"]))
    };
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