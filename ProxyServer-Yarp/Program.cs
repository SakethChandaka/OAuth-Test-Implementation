using ProxyServer_Yarp.Services;

namespace ProxyServer_Yarp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<WallpaperService>();
            builder.Services.AddScoped<WallpaperService>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
