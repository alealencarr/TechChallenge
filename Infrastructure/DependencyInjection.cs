﻿using HealthChecks.UI.Client;
using Infrastructure.DbContexts;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var cnnStr = configuration.GetConnectionString(Configuration.ConnectionString);
            services.AddTransient<DataSeeder>();

            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(cnnStr, options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    );
                });
            });
 
            services.AddHealthChecksUI()
                    .AddSqlServerStorage(cnnStr);

            return services;
        }

        public static IHealthChecksBuilder AddHealthDb(this IHealthChecksBuilder services, IConfiguration configuration)
        {
            services.AddSqlServer(configuration.GetConnectionString(Configuration.ConnectionString), name: "SQL Server Check", tags: new string[] { "db", "data" });
            return services;                                                // Isso é útil para saber que há um problema, mas a API ainda funciona
        }


        public static void AddHealthChecks(this WebApplication app)
        {
            app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(opt => { opt.UIPath = "/dashboard"; });

        }
    }
}
