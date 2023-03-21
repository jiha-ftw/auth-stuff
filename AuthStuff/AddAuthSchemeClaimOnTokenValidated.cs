using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

namespace AuthStuff
{
    public static class AddAuthSchemeClaimOnTokenValidated
    {
        public static OpenIdConnectEvents Create() => new OpenIdConnectEvents
        {
            OnTokenValidated = context => 
            {
                ((ClaimsIdentity)context.Principal.Identity).AddClaim(new Claim(".AuthScheme", context.Scheme.Name));

                return Task.CompletedTask;
            }
        };
    }
}
