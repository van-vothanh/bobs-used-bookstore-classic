using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Login(string redirectUri = null)
        {
            if(string.IsNullOrWhiteSpace(redirectUri)) return RedirectToAction("Index", "Home");

            return Redirect(redirectUri);
        }

        public IActionResult LogOut()
        {
            return configuration["Services:Authentication"] == "aws" ? CognitoSignOut() : LocalSignOut();
        }

        private IActionResult LocalSignOut()
        {
            Response.Cookies.Delete("LocalAuthentication");
            return RedirectToAction("Index", "Home");
        }

        private IActionResult CognitoSignOut()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");

            var domain = configuration["Authentication:Cognito:CognitoDomain"];
            var clientId = configuration["Authentication:Cognito:LocalClientId"];
            var logoutUri = $"{Request.Scheme}://{Request.Host}/";

            return Redirect($"{domain}/logout?client_id={clientId}&logout_uri={logoutUri}");
        }
    }
}
