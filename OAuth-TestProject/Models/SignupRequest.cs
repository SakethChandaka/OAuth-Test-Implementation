namespace Authentication_Server.Models
{
    public class SignupRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ClientId { get; set; }
    }
}
