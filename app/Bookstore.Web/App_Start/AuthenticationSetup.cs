using System.Security.Claims;
using System.Threading.Tasks;
using Bookstore.Domain.Customers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Bookstore.Web
{
    public static class AuthenticationSetup
    {
        public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["Services:Authentication"] == "aws")
            {
                ConfigureCognitoAuthentication(services, configuration);
            }
            else
            {
                ConfigureLocalAuthentication(services);
            }
        }

        private static void ConfigureLocalAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Authentication/Login";
                    options.LogoutPath = "/Authentication/Logout";
                });
        }

        private static void ConfigureCognitoAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.ClientId = configuration["Authentication:Cognito:LocalClientId"];
                options.MetadataAddress = configuration["Authentication:Cognito:MetadataAddress"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SaveTokens = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "cognito:username",
                    RoleClaimType = "cognito:groups"
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var service = context.HttpContext.RequestServices.GetRequiredService<ICustomerService>();
                        var identity = (ClaimsIdentity)context.Principal.Identity;

                        var dto = new CreateOrUpdateCustomerDto(
                            identity.FindFirst("sub")?.Value,
                            identity.Name,
                            identity.FindFirst(ClaimTypes.GivenName)?.Value,
                            identity.FindFirst(ClaimTypes.Surname)?.Value);

                        await service.CreateOrUpdateCustomerAsync(dto);
                    }
                };
            });
        }
    }
}
