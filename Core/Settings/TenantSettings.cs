using System;
using System.Collections.Generic;

namespace Core.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
    // The list of Tenants that are supposed to have access to the system.
    public class Tenant
    {
        public string Name { get; set; }
        public string TID { get; set; }
        public string ConnectionsString { get; set; }
    }
    // Shared Database settings
    public class Configuration
    {
        public string DBProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
