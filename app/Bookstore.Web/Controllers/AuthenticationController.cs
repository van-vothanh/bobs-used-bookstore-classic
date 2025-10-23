using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BobsBookstoreClassic.Data;

namespace Bookstore.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            var authenticationService = BookstoreConfiguration.GetSetting("Services/Authentication");

            return authenticationService == "aws" ? CognitoSignOut() : LocalSignOut();
        }

        private ActionResult LocalSignOut()
        {
            if (HttpContext.Request.Cookies["LocalAuthentication"] != null)
            {
                HttpContext.Response.Cookies.Delete("LocalAuthentication");
            }

            return RedirectToAction("Index", "Home");
        }

        private ActionResult CognitoSignOut()
        {
            if (Request.Cookies[".AspNet.Cookies"] != null)
            {
                Response.Cookies.Delete(".AspNet.Cookies");
            }

            var domain = BookstoreConfiguration.GetSetting("Authentication/Cognito/CognitoDomain");
            var clientId = BookstoreConfiguration.GetSetting("Authentication/Cognito/LocalClientId");
            var logoutUri = $"{Request.Scheme}://{Request.Host}/";

            return Redirect($"{domain}/logout?client_id={clientId}&logout_uri={logoutUri}");
        }
    }
}
