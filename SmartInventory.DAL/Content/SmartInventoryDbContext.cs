using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Content
{ 
public class SmartInventoryDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
    public SmartInventoryDbContext(DbContextOptions<SmartInventoryDbContext> options) : base(options)
    {

    }
   public  DbSet<Product> Products { get; set; }
  public  DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        public DbSet<StockTransaction> StockTransactions { get; set; }

        public DbSet<Sale> Sale {  get; set; }

        public DbSet<SaleDetails> SaleDetails { get; set; }

        public DbSet<Customer> Customers { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure relationships and constraints if needed
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔥 Supplier → Purchase (1 to many)
            builder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 Purchase → PurchaseDetail (1 to many)
            builder.Entity<PurchaseDetail>()
                .HasOne(d => d.Purchase)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 Product → PurchaseDetail (1 to many)
            builder.Entity<PurchaseDetail>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<SaleDetails>()
                .HasOne(d => d.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → SaleDetail
            builder.Entity<SaleDetails>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Sale>()
                 .HasOne(s => s.Customer)
                 .WithMany(c => c.Sales)
                 .HasForeignKey(s => s.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);
        }

        



    } }

