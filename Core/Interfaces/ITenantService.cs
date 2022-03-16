using Core.Settings;

namespace Core.Interfaces
{
    /// <summary>
    /// Basically, this interface should return the current DBProvider, Connection string, and Tenant Data.
    /// </summary>
    public interface ITenantService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public Tenant GetTenant();
    }
}
