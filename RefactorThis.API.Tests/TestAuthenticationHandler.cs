using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RefactorThis.API.Tests
{
    namespace MockingAuthApi.Tests
    {
        public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
        {
            public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
                UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var authenticationTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(Options.Identity),
                    new AuthenticationProperties(),
                    "Test Scheme");

                return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }
        }

        public static class TestAuthenticationExtensions
        {
            public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
            {
                return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Auth", configureOptions);
            }
        }

        public class TestAuthenticationOptions : AuthenticationSchemeOptions
        {
            public virtual ClaimsIdentity Identity { get; }
                = new ClaimsIdentity(new Claim[]
                {
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "read_product"),
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "write_product"),
                },
                "test");
        }
    }
}