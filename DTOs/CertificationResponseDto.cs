namespace PortfolioCMS.DTOs
{
    public class CertificationResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string? CredentialUrl { get; set; }
        public string? CredentialId { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool DoesNotExpire { get; set; }
        public int DisplayOrder { get; set; }
    }
}
