using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication_Server.Data;
using Authentication_Server.Models;


using Authentication_Server.Utils;
using Authentication_Server.Services;


namespace Authentication_Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ClientVerifier _clientVerifier;
        public AuthController(ApplicationDbContext context, PasswordHasher passwordHasher, TokenService tokenService, ClientVerifier clientVerifier)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService= tokenService;
            _clientVerifier = clientVerifier;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            // Validate the client credentials (you might want to check against a database)
            if (!_clientVerifier.VerifyClient(request.ClientId!, request.ClientSecret!))
            {
                return Unauthorized(new { message = "Client is Invalid or is Unregistered. Please Signup." });
            }

            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Invalid request." });
            }

            // Find the user by username
            var user = await _context.User_Auth.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Verify the password
            if (!_passwordHasher.VerifyPassword(request.Password, user.Password!))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // TODO: Generate JWT token (will implement in next steps)
            var accessToken = _tokenService.GenerateToken(request.ClientId!);
            return Ok(new { accessToken, message = "Login successful" });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {

            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Invalid request.");
            }

            // Check if user/email already exists
            var existingUser = await _context.User_Auth.FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            var newUser = new UserAuth
            {
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                Email = request.Email,
                LastUpdatedTime = DateTimeOffset.UtcNow,
                CreatedTime = DateTimeOffset.UtcNow,
            };

            try
            {
                // Add the new user to the database
                var addUser = await _context.User_Auth.AddAsync(newUser);
                
                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return success response
                return Ok(new { message = "Signup successful" });
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related errors
                return StatusCode(500, new { message = "An error occurred while saving the user.", details = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
