using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using PortfolioCMS.Attributes;
using System.Reflection;

namespace PortfolioCMS.Filters
{
    public class SwaggerApiKeyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiKeyAttribute = context.MethodInfo.DeclaringType.GetCustomAttribute<ApiKeyAttribute>();
            
            if (apiKeyAttribute != null)
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        }
    }
}