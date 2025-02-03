namespace Authentication_Server.Models
{
    public class TokenRequest
    {
        public string? AuthGrantCode { get; set; }
        public string? Username { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
