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

            builder.Services.AddSingleton<PasswordHasher>();
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<ClientVerifier>();
            builder.Services.AddScoped<HandleClientService>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
