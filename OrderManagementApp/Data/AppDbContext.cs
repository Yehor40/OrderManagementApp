using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Status> Statuses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // --- Customer (1) to Order (M) relationship ---
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();
        
        // --- Order (M) to Status (1) relationship ---
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Status)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.StatusCode) // FK is on the Order table
            .IsRequired();
        
        // --- Primary Key Configuration for Status (char) ---
        modelBuilder.Entity<Status>()
            .HasKey(s => s.Code);
        // Seed Statuses
        modelBuilder.Entity<Status>().HasData(
            new Status { Code = 'P', StatusName = "Processing" },
            new Status { Code = 'S', StatusName = "Shipped" },
            new Status { Code = 'C', StatusName = "Canceled" }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer 
            { 
                CustomerId = 1, 
                CustomerName = "John Doe", 
                Email = "john@example.com", 
                Password = "hashed_password" 
            },
            new Customer 
            { 
                CustomerId = 2, 
                CustomerName = "Jane Smith", 
                Email = "jane@example.com", 
                Password = "hashed_password" 
            },
            new Customer 
            { 
                CustomerId = 3, 
                CustomerName = "Acme Corporation LLC", 
                Email = "contact@acme.com", 
                Password = "hashed_password" 
            }
        );

        // Seed Order
        modelBuilder.Entity<Order>().HasData(
            new Order 
            { 
                OrderId = 1, 
                CustomerName = "John Doe", // Matches the Customer Name for consistency
                Date = DateTime.Now, 
                Amount = 1500, 
                CustomerId = 1, 
                StatusCode = 'P' 
            }
        );
        
    }
}
    
