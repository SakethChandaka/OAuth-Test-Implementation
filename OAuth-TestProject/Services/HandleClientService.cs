
using Authentication_Server.Data;
using Authentication_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Authentication_Server.Services
{
    public class HandleClientService
    {
        private readonly ApplicationDbContext _context;
        public HandleClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public object AddClient(string name)
        {
            // Check if the client already exists
            var existingClient = _context.Client_Auth.FirstOrDefault(c => c.Name == name);

            // If client exists, return the existing client secret
            if (existingClient != null)
            {
                throw new InvalidOperationException($"Client with Name '{name}' already exists.");
            }


            // Generate a client secret
            var clientInfo = GenerateClientInfo();

            // Create a new client entity
            var newClient = new ClientAuth
            {
                ClientId = clientInfo.clientId,
                ClientSecret = clientInfo.clientSecret,
                Name = name
            };

            // Add the new client to the database
            _context.Client_Auth.Add(newClient);
            _context.SaveChanges();

            // Return the generated client secret
            return new { clientInfo.clientId, clientInfo.clientSecret };
        }
        private (string clientId, string clientSecret) GenerateClientInfo()
        {
            // Generate a random Client ID (using GUID in this case)
            var clientId = Guid.NewGuid().ToString("N"); // "N" format removes hyphens

            // Generate a random Client Secret
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] secretBytes = new byte[32]; // 256-bit client secret
                rng.GetBytes(secretBytes);
                var clientSecret = Convert.ToBase64String(secretBytes);

                return (clientId, clientSecret);
            }
        }

    }
}
