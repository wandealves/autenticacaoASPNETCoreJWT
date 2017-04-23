using AuthAPI.Domain.Models;
using AuthAPI.Infraestructure.Data.Map;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Infraestructure.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserMap(modelBuilder.Entity<User>().ToTable("TB_User"));
        }

    }
}
