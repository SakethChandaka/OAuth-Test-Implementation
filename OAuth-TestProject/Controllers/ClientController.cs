using Authentication_Server.Data;
using Authentication_Server.Services;
using Authentication_Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Authentication_Server.Models;

namespace Authentication_Server.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientVerifier _clientVerifier;
        private readonly HandleClientService _handleClientService;
        public ClientController(ApplicationDbContext context, ClientVerifier clientVerifier, HandleClientService handleClientService)
        {
            _context = context;
            _clientVerifier = clientVerifier;
            _handleClientService = handleClientService;
        }

        [HttpPost("newclient")]
        public IActionResult CreateClient([FromBody] RegisterRequest request)
        {
            try
            {
                // Call the AddClient method from HandleClientService
                var client = _handleClientService.AddClient(request.Name);

                if (client == null)
                {
                    // Handle case where client could not be created (if needed)
                    return BadRequest("Failed to create client.");
                }


                // Return the strongly-typed ClientInfo object
                return Ok(client);
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where the client already exists
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                return StatusCode(500, new { message = "An error occurred while creating the client.", error = ex.Message });
            }
        }

    }
}
