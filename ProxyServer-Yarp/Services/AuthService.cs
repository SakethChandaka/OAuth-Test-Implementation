using Newtonsoft.Json;
using ProxyServer_Yarp.Models;
using System.Text;

namespace ProxyServer_Yarp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // This method fetches Access Token from Auth Server
        public async Task<string> LoginUser(string clientId, string clientSecret, string username, string password)
        {
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password,
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            // Send a GET request to the Auth server to fetch access token
            var response = await _httpClient.PostAsync("https://localhost:7273/api/auth/login", content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            return responseData;
        }
    }
}
