using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace AuthStuff.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureAdAuth(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddMicrosoftIdentityWebAppAuthentication(configurationRoot);

            return services.Configure<MicrosoftIdentityOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
                options.Events = AddAuthSchemeClaimOnTokenValidated.Create(OpenIdConnectDefaults.AuthenticationScheme)
            );
        }

        public static IServiceCollection AddAuth0Auth(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddAuth0WebAppAuthentication(Auth0Constants.AuthenticationScheme, options =>
            {
                options.CallbackPath = "/callback-auth0";
                options.ClientId = configurationRoot["Auth0:ClientId"];
                options.CookieAuthenticationScheme = "auth0";
                options.Domain = configurationRoot["Auth0:Domain"];
                options.OpenIdConnectEvents = AddAuthSchemeClaimOnTokenValidated.Create(Auth0Constants.AuthenticationScheme);
            });

            return services;
        }

        public static IServiceCollection SetupAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(
                        OpenIdConnectDefaults.AuthenticationScheme,
                        Auth0Constants.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        public static IServiceCollection SetupAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            return services;
        }
    }
}
