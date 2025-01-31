using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Authentication_Server.Data;
using Authentication_Server.Utils;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Server.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string clientId, string userName)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientId.ToString()), // Subject (client)
                new Claim(JwtRegisteredClaimNames.PreferredUsername, userName.ToString()), // Subject (user )
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID (unique identifier)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)); // Secret key from appsettings.json
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // From appsettings.json
                audience: _configuration["Jwt:Audience"], // From appsettings.json
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Set token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Return the token as a string
        }
    }
}
