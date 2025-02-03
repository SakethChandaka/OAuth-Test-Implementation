using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Authentication_Server.Data;
using Authentication_Server.Models;


using Authentication_Server.Utils;
using Authentication_Server.Services;
using System.Text;



namespace Authentication_Server.Controllers
{
    [ApiController]

    [Route("client/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ClientVerifier _clientVerifier;
        private readonly AuthService _authService;
        private readonly TokenRequestVerifier _tokenRequestVerifier;
        private readonly GrantService _grantService;

        public AuthController(ApplicationDbContext context, PasswordHasher passwordHasher, TokenService tokenService, ClientVerifier clientVerifier, AuthService authService, TokenRequestVerifier tokenRequestVerifier, GrantService grantService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _clientVerifier = clientVerifier;
            _authService = authService;
            _tokenRequestVerifier = tokenRequestVerifier;
            _grantService = grantService;
        }

        [HttpGet("login")]
        public IActionResult LoginPage([FromQuery] string client_id, [FromQuery] string redirect_uri)
        {
            // Validate client_id and redirect_uri (optional security measure)
            if (!_clientVerifier.VerifyClient_Login(client_id))
            {
                return BadRequest("Invalid client.");
            }

            // Serve the login page with client_id and redirect_uri
            var html = System.IO.File.ReadAllText("wwwroot/login.html")
                .Replace("client_id_from_query", client_id)
                .Replace("redirect_uri_from_query", redirect_uri);

            return Content(html, "text/html");
        }


        [HttpPost("authorize")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {

            // Get the client_id and client_secret from the Authorization header
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic "))
            {
                return Unauthorized(new { message = "Missing or invalid client credentials." });
            }

            // Extract client_id and client_secret from the base64 encoded value
            var encodedCredentials = authorizationHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var credentials = decodedCredentials.Split(':');

            if (credentials.Length != 2)
            {
                return Unauthorized(new { message = "Invalid client credentials format." });
            }

            var clientId = credentials[0];
            var clientSecret = credentials[1];

            // Validate client credentials
            var client = await _context.Client_Auth.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null || client.ClientSecret != clientSecret)
            {
                return Unauthorized(new { message = "Invalid client credentials." });
            }


            var grant = await _grantService.GenerateGrant(clientId,clientSecret, request.Username);
            if (!grant.status)
            {
                return StatusCode(500, new { message = "An exception occurred while processing your request." });
            }

            return Ok(new { grantCode = grant.code });
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] TokenRequest request)
        {

            var verifyGrant = await _tokenRequestVerifier.VerifyAuthGrantAsync(request.AuthGrantCode, request.ClientId, request.ClientSecret);

            if (!verifyGrant)
            {
                return Unauthorized(new { message = "Invalid authorization grant." });
            }

            var token = _tokenService.GenerateToken(request.ClientId,request.Username);
            if (token == null)
            {
                return BadRequest(new { message = "Error generating token, try again!." });
            }
            return Ok(new { accessToken = token , message = "token issued!"});
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var (success, message) = await _authService.SignupAsync(request);
            if (!success)
            {
                return message == "User already exists."
                    ? Conflict(new { message })
                    : BadRequest(new { message });
            }

            return Ok(new { message });
        }
    }
}
