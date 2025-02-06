namespace Authentication_Server.Models
{
    public class TokenRequest
    {
        public string AuthGrantCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
