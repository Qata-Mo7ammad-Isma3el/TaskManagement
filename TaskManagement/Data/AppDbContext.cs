using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Using TaskManagementDB as requested
            string connectionString = "Server=localhost;Database=TaskManagementDB;Trusted_Connection=True;TrustServerCertificate=true;";

            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(c => c.UserId);

                entity.Property(c => c.UserId)
                    .HasMaxLength(36);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(c => c.Email)
                    .IsUnique();

                entity.Property(c => c.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(c => c.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(c => c.IsActive)
                    .HasDefaultValue(true);

                entity.Property(c => c.LoyaltyPoints)
                    .HasDefaultValue(0);

                // Relationships
                entity.HasMany(c => c.Orders)
                    .WithOne(o => o.Customer)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasMaxLength(36);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Description)
                    .HasMaxLength(2000);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(p => p.StockQuantity)
                    .HasDefaultValue(0);

                entity.Property(p => p.IsActive)
                    .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(p => p.Name);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.OrderId);

                entity.Property(o => o.OrderId)
                    .HasMaxLength(36);

                entity.Property(o => o.CustomerId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(o => o.OrderDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(o => o.TaxAmount)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(o => o.DiscountAmount)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(o => o.FinalAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasDefaultValue(OrderStatus.Pending);

                entity.Property(o => o.ShippedDate)
                    .IsRequired(false);

                entity.Property(o => o.DeliveredDate)
                    .IsRequired(false);

                // Relationships
                entity.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(oi => oi.OrderItemId);

                entity.Property(oi => oi.OrderItemId)
                    .ValueGeneratedOnAdd();

                entity.Property(oi => oi.OrderId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(oi => oi.ProductId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(oi => oi.Quantity)
                    .IsRequired();

                entity.Property(oi => oi.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(oi => oi.Subtotal)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Indexes
                entity.HasIndex(oi => oi.OrderId);
                entity.HasIndex(oi => oi.ProductId);

                // Relationships
                entity.HasOne(oi => oi.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Product)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}