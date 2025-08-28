using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PortfolioCMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }
        [StringLength(50)]
        public new string? Email { get; set; }

        [StringLength(15)]
        public new string PhoneNumber { get; set; } = string.Empty;
        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }

        [StringLength(100)]
        public string? Headline { get; set; }

        [StringLength(1000)]
        public string? Summary { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
        public ICollection<Language> Languages { get; set; } = new List<Language>();
        public ICollection<SocialLinks> SocialLinks { get; set; } = new List<SocialLinks>();
        public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
    }
}
