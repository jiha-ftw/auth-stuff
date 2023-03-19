using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AuthStuff.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SetupRoutes(this IApplicationBuilder app)
        {
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGet("/login", async (HttpContext context, string scheme) =>
                    await context.ChallengeAsync(scheme, new LoginAuthenticationPropertiesBuilder()
                        .WithRedirectUri("/Home/LoggedIn")
                        .Build()
                ));

                endpoints.MapGet("/logout", async context =>
                {
                    var scheme = context.User.FindFirstValue(".AuthScheme");
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await context.SignOutAsync(scheme, new LogoutAuthenticationPropertiesBuilder().WithRedirectUri("/logged-out").Build());
                });

                endpoints.MapGet("/logged-out", () => "You've logged out");
            });
        }
    }
}
