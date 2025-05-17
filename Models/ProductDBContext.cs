using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VegEaseBackend.Models;

public partial class ProductDBContext : DbContext
{
    public ProductDBContext()
    {
    }

    public ProductDBContext(DbContextOptions<ProductDBContext> options)
        : base(options)
    {
    }
    public DbSet<Users> Users { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DbSet<Admin> Admins { get; set; }

    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<OrderProductMapping> OrderProductMapping { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
/*                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
*/                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });


        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC075DFFAF2E");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");


        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC075DFFAF2E");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CommentText).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(d => d.PId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Comments_Products");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Carts"); 

            entity.HasKey(e => e.Id).HasName("PK_Carts");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.IsOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Product)
                  .WithMany(p => p.Carts)
                  .HasForeignKey(d => d.ProductId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Carts_Products");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");

           
        });
        modelBuilder.Entity<OrderProductMapping>(entity =>
            {
                entity.HasKey(e => e.OPId);

                entity.Property(e => e.OPId).ValueGeneratedOnAdd();

               

                entity.HasOne(d => d.OrderDetails)
                      .WithMany(od => od.OrderProductMappings)
                      .HasForeignKey(d => d.OrderId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_OrderProductMapping_OrderDetails");
            });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
