using Microsoft.AspNetCore.Mvc;
using ProxyServer_Yarp.Services;
using ProxyServer_Yarp.Models;

namespace ProxyServer_Yarp.Controllers
{

    [ApiController]

    [Route("proxy/auth")]
    public class AuthGateway : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthGateway(AuthService authService)
        {
            _authService = authService;
        }

        // Route to login
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginUser(request.ClientId!, request.ClientSecret!, request.Username!, request.Password!);
            return Ok(response);
        }

    }

    [Route("proxy/resource")]
    public class ResourceGateway : ControllerBase
    {
        private readonly ImageService _imageService;

        public ResourceGateway(ImageService imageService)
        {
            _imageService = imageService;
        }

        // Route to Image DB
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(string id)
        {
            var image = await _imageService.GetImageAsync(id);
            return File(image, "image/jpeg");
        }

    }



}
