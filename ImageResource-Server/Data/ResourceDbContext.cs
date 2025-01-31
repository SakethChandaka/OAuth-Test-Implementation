using Microsoft.EntityFrameworkCore;
using ResourceServer_Images.Models;

namespace ResourceServer_Images.Data
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }

        public DbSet<ImageMap> Image_Map { get; set; }
        public DbSet<AccessMap> Access_Map { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ImageMap>().ToTable("Image_Map");
            modelBuilder.Entity<AccessMap>().ToTable("Image_Access_Data");
        }
    }
}
