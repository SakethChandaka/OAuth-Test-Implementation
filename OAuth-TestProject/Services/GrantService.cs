
using System.Text;
using Authentication_Server.Data;
using System.Security.Cryptography;
using Authentication_Server.Models;

namespace Authentication_Server.Services
{
    public class GrantService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public GrantService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<(string code, bool status)> GenerateGrant(string clientId, string clientSecret, string userName)
        {

            var baseString = $"{clientId}:{userName}:{clientSecret}:{DateTimeOffset.UtcNow.Ticks}";

            // Generate a secure hash of the base string (using SHA-256 in this case)
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                var grantCode = Convert.ToBase64String(hashBytes);

                // Optionally, you can trim the base64 string to a desired length, e.g., 32 characters
                grantCode = grantCode.Substring(0, 32); // Example: first 32 chars of the hash as the grant

                // Create an AuthGrant object to save to the database
                var authGrant = new AuthGrant
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    AuthGrantCode = grantCode,
                    CreatedTime = DateTimeOffset.UtcNow,
                    ExpiryTime = DateTimeOffset.UtcNow.AddHours(1), // Example: expires in 1 hour
                    IsValid = true
                };

                // Add the grant record to the database
                var add = await _context.Authorization_Grant.AddAsync(authGrant);
                var save = await _context.SaveChangesAsync(); // Commit to the database

                if (save == 0)
                {
                    throw new Exception("Failed to save authorization grant.");
                }
                return ( grantCode, true ); // Return the generated grant code
            }
        }
    }
}
