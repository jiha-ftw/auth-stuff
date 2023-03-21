using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthStuff.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult LoggedIn() => View();

        public IActionResult LoginAd() => Challenge(
            new LoginAuthenticationPropertiesBuilder().WithRedirectUri("/Home/LoggedIn").Build(),
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }
}
