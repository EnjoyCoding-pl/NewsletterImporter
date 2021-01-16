using System.Text.Json.Serialization;

namespace NewsletterImporter.Infrastructure.Models
{
    public class EmailStatusDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("did_you_mean")]
        public string EmailSpellcheck { get; set; }

        [JsonPropertyName("format_valid")]
        public bool IsFormatValid { get; set; }

        [JsonPropertyName("mx_found")]
        public bool IsMxFound { get; set; }
    }
}