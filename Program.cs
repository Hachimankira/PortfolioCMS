using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Implementation;
using PortfolioCMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// 4. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // If using authentication cookies/tokens
    });
});
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
});
// cmt this out if AutoMapper.Extensions.Microsoft.DependencyInjection this package is installed.
// builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
// 5. Configure the HTTP request pipeline.
app.UseCors("AllowFrontend");
app.MapIdentityApi<ApplicationUser>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Print all routes to console
var dataSource = app.Services.GetRequiredService<EndpointDataSource>();
foreach (var endpoint in dataSource.Endpoints)
{
    Console.WriteLine(endpoint.DisplayName);
}
app.Run();

