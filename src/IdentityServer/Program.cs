// https://gowthamcbe.com/2022/12/10/get-start-with-identity-server-4-with-asp-net-core-6/

using Microsoft.EntityFrameworkCore;
using IdentityServer.Data;

namespace IdentityServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<IdentityServerContext>(options =>
        options.UseInMemoryDatabase("IdentityDb"));

        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityConfiguration.Ids)
            .AddInMemoryApiResources(IdentityConfiguration.Apis)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddDeveloperSigningCredential();

        var app = builder.Build();
        app.MapGet("/", () => "Hello World!");
        app.UseRouting();
        app.UseIdentityServer();

        app.Run();
    }
}