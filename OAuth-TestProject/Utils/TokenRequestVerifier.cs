using Authentication_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Server.Utils
{
    public class TokenRequestVerifier
    {
        private readonly ApplicationDbContext _context;

        public TokenRequestVerifier(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerifyAuthGrantAsync(string authGrant, string clientId, string clientSecret)
        {
            // Find the client by clientId
            var client = await _context.Client_Auth.FirstOrDefaultAsync(c => c.ClientId == clientId);

            // If no client is found, return false
            if (client == null)
            {
                return false;
            }

            // Validate clientSecret
            if (client.ClientSecret != clientSecret)
            {
                return false;
            }

            // Optional: Verify the authorization grant (authGrant) - this depends on your specific grant implementation
            var verifyValidity = await _context.Authorization_Grant.FirstOrDefaultAsync(g => g.AuthGrantCode == authGrant);
            if ((!verifyValidity!.IsValid))
            {
                return false;
            }
            // For example, if the authorization grant is a code, you would verify if it's valid.
            var authGrantRecord = await _context.Authorization_Grant
                .Where(ag => ag.AuthGrantCode == authGrant && ag.ClientId == clientId)
                .FirstOrDefaultAsync();

            // Check if the record exists and if the client secret matches
            if (authGrantRecord != null && authGrantRecord.ClientSecret == clientSecret)
            {
                // Optionally, check expiration or validity
                if (authGrantRecord.ExpiryTime.HasValue && authGrantRecord.ExpiryTime.Value < DateTimeOffset.UtcNow)
                {
                    return false; // Expired grant
                }

                return true; // Valid grant
            }

            return false; // Invalid grant

        }
    }
}
