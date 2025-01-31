using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceServer_Images.Data;
using ResourceServer_Images.Models;
using System.Net.Http;
using System.Net.Mime;

namespace ResourceServer_Images.Services
{
    public class ImageFetcher
    {
        private readonly ResourceDbContext _context;
        private readonly HttpClient _httpClient;

        public ImageFetcher(ResourceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<FileContentResult?> GetImage(string imageId, string clientId, string username)
        {
            try
            {
                var imageUrl = await FetchImage(imageId);
                if (imageUrl != null)
                {
                    await AddEntry(clientId, username, imageId);
                    var downloadImage = await _httpClient.GetByteArrayAsync(imageUrl);
                    return new FileContentResult(downloadImage, MediaTypeNames.Image.Jpeg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching image: {ex.Message}");
            }
            return null;
        }

        public async Task<string?> FetchImage(string imageId)
        {
            var image = await _context.Image_Map.FirstOrDefaultAsync(img => img.ImageId == imageId);
            return image?.ImageURL;
        }

        public async Task<AccessMap> AddEntry(string clientId, string username, string imageId)
        {
            var accessMap = new AccessMap
            {
                ClientId = clientId,
                User = username,
                ImageId = imageId,
                AccessTime = DateTime.UtcNow
            };

            _context.Access_Map.Add(accessMap);
            await _context.SaveChangesAsync();
            return accessMap;
        }
    }
}
