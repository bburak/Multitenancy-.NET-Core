using Core.Settings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
        {
            // Gets default settings from appsettings.json file
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;

            if (defaultDbProvider.ToLower() == "mssql")
            {
                // Registers ApplicationDbContext using the SQLServer package.
                services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            var tenants = options.Tenants;

            // If a tenant has no Connection String declared, it assigns the
            // default connection string to it.
            foreach (var tenant in tenants)
            {
                string connectionString;
                if (string.IsNullOrEmpty(tenant.ConnectionsString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant.ConnectionsString;
                }
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }

        /// <summary>
        /// Get the configuration from AppSettings.json in a static file.
        /// </summary>
        /// <returns></returns>
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}
