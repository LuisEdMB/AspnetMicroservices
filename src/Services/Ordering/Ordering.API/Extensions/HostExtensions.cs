using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, 
            Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(DbContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(DbContext).Name);
                }
                catch (SqlException exception)
                {
                    logger.LogError(exception, "An error occurred while migrating the database used on context {DbContextName}", 
                        typeof(DbContext).Name);
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase(host, seeder, retryForAvailability);
                    }
                }
            }
            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, 
            TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
