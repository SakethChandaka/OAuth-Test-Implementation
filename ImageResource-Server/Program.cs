using Microsoft.EntityFrameworkCore;
using ResourceServer_Images.Data;
using ResourceServer_Images.Services;

namespace ResourceServer_Images
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add database context
            builder.Services.AddDbContext<ResourceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHttpClient<ImageFetcher>();
            builder.Services.AddScoped<ImageFetcher>();
            builder.Services.AddSingleton<TokenValidator>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
