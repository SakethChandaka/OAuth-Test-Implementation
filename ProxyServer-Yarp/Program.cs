using ProxyServer_Yarp.Services;

namespace ProxyServer_Yarp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<AuthService>();
            builder.Services.AddHttpClient<ImageService>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClientApp", policy =>
                {
                    policy.WithOrigins("https://localhost:7156") // Client app URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowClientApp");
            // Configure middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
