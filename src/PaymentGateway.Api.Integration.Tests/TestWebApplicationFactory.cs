using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PaymentGateway.Api.Integration.Tests
{
    // TODO: Instantiate TestIdWebApplicationFactory on port 7205 or fix MockJwtTokens 
    public class TestWebApplicationFactory : WebApplicationFactory<PaymentGateway.Api.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.ConfigureAppConfiguration((context, conf) =>
            //{
            //    conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            //});

            builder.ConfigureTestServices(ConfigureServices);
            builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    var config = new OpenIdConnectConfiguration()
            //    {
            //        Issuer = MockJwtTokens.Issuer, 
            //    };

            //    config.SigningKeys.Add(MockJwtTokens.SecurityKey);
            //    options.Configuration = config;
            //});
        }
    }

    //public static class MockJwtTokens
    //{
    //    public static string Issuer { get; } = "PaymentGateway.Api";
    //    public static SecurityKey SecurityKey { get; }
    //    public static SigningCredentials SigningCredentials { get; }

    //    private static readonly JwtSecurityTokenHandler s_tokenHandler = new JwtSecurityTokenHandler();
    //    private static readonly RandomNumberGenerator s_rng = RandomNumberGenerator.Create();
    //    private static readonly byte[] s_key = new byte[32];

    //    static MockJwtTokens()
    //    {
    //        s_rng.GetBytes(s_key);
    //        SecurityKey = new SymmetricSecurityKey(s_key) { KeyId = Guid.NewGuid().ToString() };
    //        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    //    }

    //    public static string GenerateJwtToken(IEnumerable<Claim> claims)
    //    {
    //        return s_tokenHandler.WriteToken(new JwtSecurityToken(Issuer, null, claims, null, DateTime.UtcNow.AddMinutes(20), SigningCredentials));
    //    }
    //}
}
