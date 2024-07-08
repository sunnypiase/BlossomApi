using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using BlossomApi.Seeders;

namespace BlossomApi.DB
{
    public sealed class BlossomContext : DbContext
    {
        public BlossomContext(DbContextOptions<BlossomContext> options) : base(options)
        {
            try
            {
                if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
                if (!databaseCreator.CanConnect())
                {
                    databaseCreator.Create();
                }
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-one relationship between User and SiteUser
            modelBuilder.Entity<User>()
                .HasOne(u => u.SiteUser)
                .WithOne(su => su.User)
                .HasForeignKey<SiteUser>(su => su.UserId);

            // Many-to-one relationship between ShoppingCart and User
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            // Many-to-many relationship between Product and Category using a join table
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"))
                .HasData(DatabaseProductCategorySeeder.GetProductCategoryConnections());

            // Configure decimal property precision and scale
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(18,2)");

            // Map serialized properties to specific column names
            modelBuilder.Entity<Product>()
                .Property(p => p.ImagesSerialized)
                .HasColumnName("Images");

            modelBuilder.Entity<Product>()
                .Property(p => p.OptionsSerialized)
                .HasColumnName("Options");

            modelBuilder.Entity<Product>()
                .Property(p => p.DieNumbersSerialized)
                .HasColumnName("DieNumbers");

            // Many-to-one relationship between ShoppingCartProduct and ShoppingCart
            modelBuilder.Entity<ShoppingCartProduct>()
                .HasOne(scp => scp.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartProducts)
                .HasForeignKey(scp => scp.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            // Many-to-one relationship between ShoppingCartProduct and Product
            modelBuilder.Entity<ShoppingCartProduct>()
                .HasOne(scp => scp.Product)
                .WithMany(p => p.ShoppingCartProducts)
                .HasForeignKey(scp => scp.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            // One-to-one relationship between Order and ShoppingCart
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShoppingCart)
                .WithOne(sc => sc.Order)
                .HasForeignKey<Order>(o => o.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade); // Default behavior

            // One-to-one relationship between Order and DeliveryInfo
            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryInfo)
                .WithOne(di => di.Order)
                .HasForeignKey<DeliveryInfo>(di => di.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Default behavior

            // One-to-many relationship between Product and Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Default behavior

            // Many-to-many relationship between Product and Characteristic using a join table
            modelBuilder.Entity<Characteristic>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Characteristics)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCharacteristic",
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    j => j.HasOne<Characteristic>().WithMany().HasForeignKey("CharacteristicId"));
            modelBuilder.Entity<Category>().HasData(DatabaseCategorySeeder.GetCategories());
            modelBuilder.Entity<Product>().HasData(DatabaseProductSeeder.GetProducts());
        }
    }
}
