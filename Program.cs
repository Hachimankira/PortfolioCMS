using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Filters;
using PortfolioCMS.Middleware;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Implementation;
using PortfolioCMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
            npgsqlOptions.CommandTimeout(60);
        }));

// 2. Add authorization & identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
       .AddEntityFrameworkStores<AppDbContext>();

// 3. Register services
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICertification, CertificationService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISocialLinksService, SocialLinksService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

// 4. Add controllers
builder.Services.AddControllers();

// 5. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CMSPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://portfolio-cms-front.vercel.app",
                "https://www.portfolio-cms-front.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 6. Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("PublicApiRateLimitPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Request.Headers["X-API-Key"].FirstOrDefault()
                          ?? context.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous",
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

// 7. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PortfolioCMS API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });

    options.AddSecurityDefinition("ApiKey", new()
    {
        Name = "X-API-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "API Key for accessing public portfolio endpoints.",
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            Array.Empty<string>()
        }
    });

    options.OperationFilter<SwaggerApiKeyFilter>();
});

var app = builder.Build();

// 8. Middleware order matters
// Handle OPTIONS requests for preflight
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});
// Enable CORS first
app.UseCors("CMSPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();


// Authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// Rate limiter
app.UseRateLimiter();

// Public API middleware (API key + rate limiting)
app.UseWhen(context => context.Request.Path.StartsWithSegments("/public"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});

// Map endpoints
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

// Redirect root to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
