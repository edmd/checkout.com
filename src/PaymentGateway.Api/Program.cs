using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Api.Middleware;
using PaymentGateway.Data.IoC;
using Infrastructure.Transaction.Services;

namespace PaymentGateway.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var identityServerUrl = builder.Configuration.GetValue<string>("IdentityServerUrl");

        // Add services to the container.
        builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
        {
            options.Authority = identityServerUrl; // "https://localhost:7205";
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false
            };
        });

        builder.Services.AddLogging();
        builder.Services.RegisterServices();
        builder.Services.RegistraterDataServices();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<ExceptionMiddleware>>();
        app.UseMiddleware<ExceptionMiddleware>();

        //app.ConfigureExceptionHandler(logger);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();

    }
}