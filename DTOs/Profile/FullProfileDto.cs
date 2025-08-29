using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs
{
    public class FullProfileDto
    {
        // Profile fields (from GetProfileDto)
        // public string? Id { get; set; }
        [StringLength(100)]
        public string? UserName { get; set; }
        [StringLength(100)]
        public string? FullName { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }
        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        [Url]
        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }
        [StringLength(100)]
        public string? Headline { get; set; }
        [StringLength(1000)]
        public string? Summary { get; set; }
        [StringLength(100)]
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Related data
        public List<CertificationResponseDto> Certifications { get; set; } = new();
        public List<EducationResponseDto> Educations { get; set; } = new();
        // Add more as needed, e.g., public List<ProjectResponseDto> Projects { get; set; } = new();
    }
}