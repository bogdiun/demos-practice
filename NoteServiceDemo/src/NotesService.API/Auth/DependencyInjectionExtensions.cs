namespace NotesService.API.Auth;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Identity / Authentication, to be replaced by something else like OAuth and might movoe client side
        services.AddDbContext<AuthDataContext>(o => o.UseSqlServer(configuration.GetConnectionString("AuthenticationDB")));
        services.AddIdentity<IdentityUser, IdentityRole>(o =>
                {
                    o.Password.RequiredLength = 8;
                    o.Password.RequiredUniqueChars = 2;
                })
                .AddEntityFrameworkStores<AuthDataContext>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.ConfigurationKey));

        JwtSettings jwtSettings = new();

        configuration.Bind(JwtSettings.ConfigurationKey, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => o.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                });
        return services;
    }

    public static async Task RunAuthDatabaseMigrationAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuthDataContext>();

        await dbContext.Database.MigrateAsync();
    }
}
