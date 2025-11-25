using CryptoLink.Application.Persistance;
using CryptoLink.Architecture.Database;
using CryptoLink.Architecture.Repositories;
using CryptoLink.Architecture.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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



            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookWordRepository, BookWordRepository>();
            services.AddScoped<ILinkExtendedRepository, LinkExtendedRepository>();


/*            services.AddScoped<IEmailService, EmailService>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

            services.ConfigureOptions<JwtOptionsSetup>();
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.ConfigureOptions<EmailOptionsSetup>();

            services.AddAuthorization();

            services.AddHttpContextAccessor();

            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            .WithIdentity("ProcessOutboxMessagesJob-trigger")
                            .WithSimpleSchedule(schedule =>
                                schedule
                                    .WithIntervalInSeconds(10)
                                    .RepeatForever())
                    );

            });

            services.AddQuartzHostedService();*/

            return services;
        }


     }
}
