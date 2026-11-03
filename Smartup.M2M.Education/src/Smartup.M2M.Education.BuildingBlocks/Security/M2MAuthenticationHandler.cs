using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Smartup.M2M.Education.BuildingBlocks.Security
{
    public class M2MAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "M2MClientAuth";
        public string ClientId { get; set; }=string.Empty;
        public string ClientSecret { get; set; }=string.Empty;
    }
    public class M2MAuthenticationHandler : AuthenticationHandler<M2MAuthenticationOptions>
    {
        public M2MAuthenticationHandler(
            IOptionsMonitor<M2MAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.TryGetValue("X-Client-Id", out var clientId) ||
            !Request.Headers.TryGetValue("X-Client-Secret", out var clientSecret))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Client Credentials"));
            }

            if (clientId != Options.ClientId || clientSecret != Options.ClientSecret)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Client Credentials"));
            }

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, clientId!) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
