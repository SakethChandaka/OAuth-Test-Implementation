
namespace ProxyServer_Yarp.Services
{
    public class WallpaperService
    {
        private readonly HttpClient _httpClient;

        public WallpaperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // This method fetches the image as byte[] from the image DB service.
        public async Task<byte[]> GetImageAsync()
        {
            // Send a GET request to the image DB service to fetch the image by ID
            var response = await _httpClient.GetAsync($"https://drive.google.com/uc?export=download&id=1WinqT1kkWkF8KJf1f789XMi_D-etOQHs");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a byte array (raw image data)
            var imageData = await response.Content.ReadAsByteArrayAsync();

            return imageData;
        }
    }
}
