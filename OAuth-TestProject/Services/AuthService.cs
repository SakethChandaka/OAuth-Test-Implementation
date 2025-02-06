using Newtonsoft.Json;
using Authentication_Server.Models;
using System.Text;
using Authentication_Server.Utils;
using Microsoft.EntityFrameworkCore;
using Authentication_Server.Data;

namespace Authentication_Server.Services
{
    public class AuthService
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ClientVerifier _clientVerifier;
        public AuthService(ApplicationDbContext context, PasswordHasher passwordHasher, TokenService tokenService, ClientVerifier clientVerifier)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _clientVerifier = clientVerifier;
        }

        public async Task<(bool Success, string Message)> LoginAsync(LoginRequest request)
        {

            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return (false, "Invalid request.");
            }

            var user = await _context.User_Auth.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            { return (false, "Invalid username or password."); }

            return (true, "Login successful");
              
        }


        public async Task<(bool Success, string Message)> SignupAsync(SignupRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return (false, "Invalid request.");

            var existingUser = await _context.User_Auth
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (existingUser != null)
                return (false, "User already exists.");

            var newUser = new UserAuth
            {
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                Email = request.Email,
                LastUpdatedTime = DateTimeOffset.UtcNow,
                CreatedTime = DateTimeOffset.UtcNow
            };

            try
            {
                await _context.User_Auth.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return (true, "Signup successful");
            }
            catch (DbUpdateException)
            {
                return (false, "An error occurred while saving the user.");
            }
            catch (Exception)
            {
                return (false, "An unexpected error occurred.");
            }
        }

    }
}
