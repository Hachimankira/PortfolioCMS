using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Models;

namespace PortfolioCMS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Adding DbSets properties for each entity
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialLinks> SocialLinks { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }

    }
}
