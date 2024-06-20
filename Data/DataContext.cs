using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Models;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace commerce_tracker_v2.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        // public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);
            });

            // modelBuilder.Entity<Basket>(entity =>
            // {
            //     entity.HasKey(b => b.BasketId);

            //     entity.HasOne(b => b.User)
            //         .WithOne(b => b.Basket);
            // });

            // modelBuilder.Entity<BasketItem>()
            //     .HasOne(bi => bi.Basket)
            //     .WithMany(b => b.BasketItems)
            //     .HasForeignKey(bi => bi.BasketId);

            // modelBuilder.Entity<BasketItem>()
            //     .HasOne(bi => bi.Product)
            //     .WithMany()
            //     .HasForeignKey(bi => bi.ProductId);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(o => o.ProductId);
            });

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasMaxLength(256);
                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityRole>().HasData(roles);



        }


    }
}