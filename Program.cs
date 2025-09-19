using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Filters;
using PortfolioCMS.Middleware;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Implementation;
using PortfolioCMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// for postgresql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions => 
        {
            // Add connection resilience with retries
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            
            // Increase command timeout for potentially slow operations
            npgsqlOptions.CommandTimeout(60);
        }));

// 2. Add authorization
builder.Services.AddAuthorization();
// 3. Add Identity 
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Register your custom services
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICertification, CertificationService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISocialLinksService, SocialLinksService>();
// builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();

builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

// 4. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    // CMS Dashboard CORS policy - restricted to specific origins
    options.AddPolicy("CMSPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://portfolio-cms-front.vercel.app",
                "https://www.portfolio-cms-front.vercel.app"
            )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for cookies
    });

    // Public API CORS policy - allows any origin for read-only operations
    options.AddPolicy("PublicApiPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods("GET")
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "PortfolioCMS API", Version = "v1" });

    // JWT Bearer authentication for admin dashboard endpoints
    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });

    // API Key authentication for public portfolio endpoints
    options.AddSecurityDefinition("ApiKey", new()
    {
        Name = "X-API-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "API Key for accessing public portfolio endpoints.",
    });

    // Configure security requirements
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add operation filter to apply API key security to endpoints with [ApiKey] attribute
    options.OperationFilter<SwaggerApiKeyFilter>();
});
// cmt this out if AutoMapper.Extensions.Microsoft.DependencyInjection this package is installed.
// builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
// 5. Configure the HTTP request pipeline.
app.UseCors();
app.MapIdentityApi<ApplicationUser>();
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }


// Always enable Swagger for now (you can restrict it later)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add this before app.MapControllers()
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/public"),
    appBuilder => appBuilder.UseMiddleware<ApiKeyMiddleware>()
);
app.MapControllers();

// Add this after app.Build() but before app.Run()
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Try to connect
        bool canConnect = dbContext.Database.CanConnect();
        Console.WriteLine($"Database connection test: {(canConnect ? "Successful" : "Failed")}");
        
        if (!canConnect)
        {
            Console.WriteLine("Warning: Database connection failed. Application may not function correctly.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection error: {ex.Message}");
    }
}
// Add this before app.Run()
app.MapGet("/", () => Results.Redirect("/swagger"));
// Add this before app.Run()
app.MapGet("/health", () => "OK");
app.Run();

