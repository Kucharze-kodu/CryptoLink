using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Application.Utils;
using CryptoLink.Architecture.Authentication;
using CryptoLink.Architecture.Database;
using CryptoLink.Architecture.Repositories;
using CryptoLink.Architecture.Utils;
using CryptoLink.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CryptoLink.Architecture
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddArchitecture(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CryptoLinkDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    r =>
                        r.MigrationsAssembly(typeof(DependencyInjection).Assembly.ToString())));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookWordRepository, BookWordRepository>();
            services.AddScoped<ILinkExtendedRepository, LinkExtendedRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<ICryptoService, PgpCryptoService>();
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddScoped<IJwtProvider, JwtProvider>();


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
             {
                 // Twoje parametry walidacji (Issuer, Key itp.)
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         if (context.Request.Cookies.ContainsKey("CookiesAuth"))
                         {
                             context.Token = context.Request.Cookies["CookiesAuth"];
                         }
                         return Task.CompletedTask;
                     }
                 };
             });

            // Konfiguracja ciasteczek dla HTTP
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });


            services.ConfigureOptions<JwtOptionsSetup>();
            services.ConfigureOptions<JwtBearerOptionsSetup>();


            services.AddAuthorization();

            services.AddHttpContextAccessor();

            // Data Protection configuration
            services.AddDataProtection()
                .PersistKeysToDbContext<CryptoLinkDbContext>()
                .SetApplicationName("CryptoLink");

            return services;
        }


     }
}
