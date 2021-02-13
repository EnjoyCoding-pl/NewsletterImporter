namespace NewsletterImporter.Domain.Models
{
    public class EmailStatus
    {
        public bool IsFormatValid { get; init; }
        public bool IsMxFound { get; init; }
        public bool IsValid => this is { IsFormatValid: true, IsMxFound: true };
    }
}