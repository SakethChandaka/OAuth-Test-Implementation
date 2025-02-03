using Microsoft.EntityFrameworkCore;
using Authentication_Server.Models;

namespace Authentication_Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserAuth> User_Auth { get; set; }
        public DbSet<ClientAuth> Client_Auth { get; set; }
        public DbSet<AuthGrant> Authorization_Grant { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserAuth>().ToTable("User_Auth");
            modelBuilder.Entity<ClientAuth>().ToTable("Client_Auth");
            modelBuilder.Entity<AuthGrant>().ToTable("Authorization_Grant");
        }
    }
}
