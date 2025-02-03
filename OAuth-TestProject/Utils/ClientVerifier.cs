using Authentication_Server.Data;

namespace Authentication_Server.Utils
{
    public class ClientVerifier
    {
        private readonly ApplicationDbContext _context;

        public ClientVerifier(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool VerifyClient_Login(string clientId)
        {
            var client = _context.Client_Auth.FirstOrDefault(c => c.ClientId == clientId);

            // If no client is found, return false
            return client != null;
        }

        public bool VerifyClient_Token(string clientId, string clientSecret)
        {
            var client = _context.Client_Auth.FirstOrDefault(c => c.ClientId == clientId && c.ClientSecret == clientSecret);

            // If no client is found or clientSecret doesn't match, return false
            return client != null;
        }
    }
}
