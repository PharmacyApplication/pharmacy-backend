using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Models;

namespace PharmacyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Inventory → Medicine (one-to-one)
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Medicine)
                .WithOne(m => m.Inventory)
                .HasForeignKey<Inventory>(i => i.MedicineId);

            // Cart → User (one-to-one)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);

           

            // Seed Department User
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                FullName = "Admin",
                Email = "admin@pharmacy.com",
                PasswordHash = "$2y$11$vpr.RUTqVQuvEZZre4qn6u1mHDJTvv8HgdB61YJ6joELxzl4L98Vy",
                Role = "Admin",
                PhoneNumber = "8888888888",
                Address = "Pharmacy Admin",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            });
        }
    }
}