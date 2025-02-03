
namespace ProxyServer_Yarp.Services
{
    public class ImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // This method fetches the image as byte[] from the image DB service.
        public async Task<byte[]> GetImageAsync(string id)
        {
            // Send a GET request to the image DB service to fetch the image by ID
            var response = await _httpClient.GetAsync($"https://localhost:7111/rgateway/request/{id}");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a byte array (raw image data)
            var imageData = await response.Content.ReadAsByteArrayAsync();

            return imageData;
        }
    }
}
