using Microsoft.AspNetCore.Mvc;

namespace PortfolioCMS.Attributes
{
    public class ApiKeyAttribute : ApiExplorerSettingsAttribute
    {
        public ApiKeyAttribute()
        {
            // This is just a marker attribute for Swagger
            // The actual authentication is handled by the middleware
        }
    }
}