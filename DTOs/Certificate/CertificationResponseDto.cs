using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs
{
    public class CertificationResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the certification")]
        public string Id { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Title of the certification")]
        public string Title { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Organization that issued the certification")]
        public string Issuer { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Public URL to verify the certification")]
        public string? CredentialUrl { get; set; }

        [SwaggerSchema(Description = "Certification credential ID")]
        public string? CredentialId { get; set; }

        [SwaggerSchema(Description = "Date when the certification was issued", Format = "date")]
        public DateTime DateIssued { get; set; }

        [SwaggerSchema(Description = "Expiration date of the certification (null if it does not expire)", Format = "date")]
        public DateTime? ExpirationDate { get; set; }

        [SwaggerSchema(Description = "Indicates whether the certification never expires")]
        public bool DoesNotExpire { get; set; }

        [SwaggerSchema(Description = "Order in which the certification is displayed")]
        public int DisplayOrder { get; set; }
    }
}
