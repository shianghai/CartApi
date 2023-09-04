using CartApi.Configurations;
using CartApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace CartApi.Data
{
    public class CartDbContext : IdentityDbContext<ApiUser, Role, long>
    {
        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options)
        {

        }

        public DbSet<CartApi.Domain.Entities.Cart> Cart { get; set; }

        public DbSet<Item> Item { get; set; }

        public DbSet<ApiUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
