using Microsoft.AspNetCore.Mvc;
using ProxyServer_Yarp.Services;

namespace ProxyServer_Yarp.Controllers
{

    [ApiController]
    [Route("api/resource")]
    public class ProxyGateway : ControllerBase
    {
        private readonly WallpaperService _wallpaperService;

        public ProxyGateway(WallpaperService wallpaperService)
        {
            _wallpaperService = wallpaperService;
        }

        // Route to Image DB
        [HttpGet("image/card")]
        public async Task<IActionResult> GetImage()
        {
            var image = await _wallpaperService.GetImageAsync();
            return File(image, "image/jpeg");
        }

    }
}
