using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiKeyService apiKeyService)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsJsonAsync(new { message = "API Key is missing" });
                return;
            }

            var apiKey = extractedApiKey.ToString();
            
            // Validate the API key against the database
            var isValid = await apiKeyService.ValidateApiKeyAsync(apiKey);
            if (!isValid)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsJsonAsync(new { message = "Invalid API Key" });
                return;
            }

            // API key is valid, add username to context for controllers to use
            var username = await apiKeyService.GetUsernameFromApiKeyAsync(apiKey);
            context.Items["Username"] = username;

            await _next(context);
        }
    }
}