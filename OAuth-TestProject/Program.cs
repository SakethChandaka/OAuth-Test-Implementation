using Microsoft.EntityFrameworkCore;
using Authentication_Server.Data;
using Authentication_Server.Utils;
using Authentication_Server.Services;

namespace Authentication_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<ClientVerifier>();
            builder.Services.AddScoped<HandleClientService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TokenRequestVerifier>();
            builder.Services.AddScoped<GrantService>();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorApp",
                    policy => policy.WithOrigins("https://localhost:7156") // Blazor app URL
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseCors("AllowBlazorApp"); // Apply the policy

            // Configure middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
