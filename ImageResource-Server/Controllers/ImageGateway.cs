using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResourceServer_Images.Models;
using ResourceServer_Images.Services;

namespace ResourceServer_Images.Controllers
{
    [ApiController]
    [Route("api/rgateway")]
    public class ImageGateway : Controller
    {
        private readonly TokenValidator _tokenValidator;
        private readonly ImageFetcher _imageFetcher;
        public ImageGateway(TokenValidator tokenValidator, ImageFetcher imageFetcher)
        {
            _tokenValidator = tokenValidator;
            _imageFetcher = imageFetcher;
        }

        [HttpPost("request/{id}")]
        public async Task<IActionResult> ImageRequest([FromBody] ImageRequest request, string id)
        {
            var access = _tokenValidator.IsTokenValid(request.AccessToken!);
            if (access.IsValid)
            {
                var image = await _imageFetcher.GetImage(id, access.ClientId!, access.Username!);
                if (image != null)
                {
                    return File(image.FileContents, image.ContentType);
                }
                else
                {
                    return NotFound("Image not found!");
                }
            }
            else
            {
                return Unauthorized("Unauthorized access! Request access again.");
            }
        }
    }
}
