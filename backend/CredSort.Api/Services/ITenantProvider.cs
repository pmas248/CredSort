namespace CredSort.Api.Services
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }

    public class TenantProvider : ITenantProvider
    {
        // This service is responsible for providing the Tenant ID for the current request.
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /*
        public Guid GetTenantId()
        {
            var tenantIdClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "tenant_id");
            if (tenantIdClaim == null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
            {
                throw new InvalidOperationException("Tenant ID claim is missing or invalid.");
            }
            return tenantId;
        }*/

        public Guid GetTenantId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return Guid.Empty;

            var claim = context.User.FindFirst("tenant_id")?.Value;
            if (Guid.TryParse(claim, out var claimId)) return claimId;

            // 2. Fallback to Header (Development/Testing approach)
            // This allows you to test in Swagger before you build the Login system
            var header = context.Request.Headers["X-Tenant-Id"].ToString();
            if (Guid.TryParse(header, out var headerId)) return headerId;

            // 3. If we are doing a Migration or no tenant is provided
            // For now, return Empty. Later, you can decide if you want to throw an error.
            return Guid.Empty;
        }
    }

}
