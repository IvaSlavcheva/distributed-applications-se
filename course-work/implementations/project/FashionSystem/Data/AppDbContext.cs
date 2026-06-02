using FashionSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace FashionSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                   : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<FashionItem> FashionItems => Set<FashionItem>();

        public DbSet<Rental> Rentals => Set<Rental>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // User -> Rentals
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // FashionItem -> Rentals
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.FashionItem)
                .WithMany(f => f.Rentals)
                .HasForeignKey(r => r.FashionItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            modelBuilder.Entity<FashionItem>()
                .Property(f => f.PricePerDay)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalPrice)
                .HasPrecision(10, 2);

            // Seed Admin
            modelBuilder.Entity<User>().HasData(

                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "Admin",
                    PhoneNumber = "0888123456",
                    CreatedAt = new DateTime(2026, 5, 19)
                },
                new User
                {
                    Id = 2,
                    FirstName = "Iva",
                    LastName = "Slavcheva",
                    Email = "iva@com",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "User",
                    PhoneNumber = "0888123459",
                    CreatedAt = new DateTime(2026, 5, 19)
                }
            );
        }
    }
}