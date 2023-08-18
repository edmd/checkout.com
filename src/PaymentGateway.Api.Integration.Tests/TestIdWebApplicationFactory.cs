using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer;

namespace PaymentGateway.Api.Integration.Tests
{
    public class TestIdWebApplicationFactory : WebApplicationFactory<IdentityServer.Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureServices);
            builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole(options => options.IncludeScopes = true);
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}