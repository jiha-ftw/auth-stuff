using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

namespace AuthStuff
{
    public static class AddAuthSchemeClaimOnTokenValidated
    {
        public static OpenIdConnectEvents Create(string scheme) => new OpenIdConnectEvents
        {
            OnTokenValidated = context => 
            {
                var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                ((ClaimsIdentity)context.Principal.Identity).AddClaim(new Claim(".AuthScheme", scheme));
            
                return Task.CompletedTask;
            }
        };
    }
}
