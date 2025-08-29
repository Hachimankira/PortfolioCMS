namespace PortfolioCMS.DTOs
{
    public class UpdateCertificationDto
    {
        public string? Title { get; set; }
        public string? Issuer { get; set; }
        public string? CredentialUrl { get; set; }
        public string? CredentialId { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? DoesNotExpire { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
