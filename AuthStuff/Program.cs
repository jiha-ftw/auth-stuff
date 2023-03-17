using Auth0.AspNetCore.Authentication;
using AuthStuff;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MicrosoftIdentityOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
    options.Events = AddAuthSchemeClaimOnTokenValidated.Create(OpenIdConnectDefaults.AuthenticationScheme)
);

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration);

builder.Services.AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, options =>
{
    options.CookieAuthenticationScheme = "auth0";
    options.CallbackPath = "/callback-auth0";
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.OpenIdConnectEvents = AddAuthSchemeClaimOnTokenValidated.Create(Auth0Constants.AuthenticationScheme);
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(
            OpenIdConnectDefaults.AuthenticationScheme,
            Auth0Constants.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddMvc();

var app = builder.Build();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}" );

app.MapGet("/login", async (HttpContext context, string scheme) =>
    await context.ChallengeAsync(scheme, new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri("/Home/LoggedIn")
        .Build()
));

app.MapGet("/logout", async context =>
{
    var scheme = context.User.FindFirstValue(".AuthScheme");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignOutAsync(scheme, new LogoutAuthenticationPropertiesBuilder().WithRedirectUri("/logged-out").Build());
});

app.MapGet("/logged-out", () => "You've logged out");

app.Run();
