using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Api.Extensions
{
    public static class HostExtensions
    {
        public static WebApplication MigrateDatabase<T>(this WebApplication application,
            Action<T,IServiceProvider> seeder,int? retry=0) where T : DbContext
        {
            int retryValue = retry.Value;

            using(var scope = application.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<T>>();
                var context = services.GetService<T>();

                try
                {
                    logger.LogInformation("Migration started for context {ContextName}",typeof(T).Name);

                    InvokeSeeder<T>(seeder,context,services);
                    logger.LogInformation("Migration finished for context {ContextName}",typeof(T).Name);

                }catch(SqlException ex)
                {
                    logger.LogError("SQL exception while migrating db",ex);

                    if (retryValue < 50)
                    {
                        retryValue++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<T>(application, seeder, retryValue);
                    }
                }
                return application;
            }
        }

        private static void InvokeSeeder<T>(Action<T, IServiceProvider> seeder,
            T context,IServiceProvider services
            ) where T:DbContext
        {
            context.Database.Migrate();
            seeder(context,services);
        }
    }
}
