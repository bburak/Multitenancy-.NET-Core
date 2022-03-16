using Core.Interfaces;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;
            // Check HTTP context is not null
            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }
            }
        }
        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.Where(a => a.TID == tenantId).FirstOrDefault();
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionsString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }
        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionsString = _tenantSettings.Defaults.ConnectionString;
        }
        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionsString;
        }
        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }
        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
