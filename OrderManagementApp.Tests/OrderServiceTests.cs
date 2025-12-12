using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Data;
using OrderManagementApp.Data.Entities;
using OrderManagementApp.Servises;
using Xunit;

namespace OrderManagementApp.Tests;

public class OrderServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        
        // Seed test data
        if (!context.Statuses.Any())
        {
            context.Statuses.AddRange(
                new Status { Code = 'P', StatusName = "Processing" },
                new Status { Code = 'S', StatusName = "Shipped" },
                new Status { Code = 'C', StatusName = "Canceled" }
            );
        }

        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { CustomerId = 1, CustomerName = "John Doe", Email = "john@example.com", Password = "pass" },
                new Customer { CustomerId = 2, CustomerName = "Jane Smith", Email = "jane@example.com", Password = "pass" },
                new Customer { CustomerId = 3, CustomerName = "Acme Corporation LLC", Email = "contact@acme.com", Password = "pass" }
            );
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldAddOrderToDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer = await context.Customers.FirstAsync();

        var newOrder = new Order
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            StatusCode = 'P',
            Amount = 2500,
            Date = DateTime.Now
        };

        // Act
        await service.CreateOrderAsync(newOrder);

        // Assert
        var orders = await context.Orders.ToListAsync();
        Assert.Single(orders);
        Assert.Equal(2500, orders[0].Amount);
        Assert.Equal(customer.CustomerId, orders[0].CustomerId);
        Assert.Equal('P', orders[0].StatusCode);
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldReturnAllOrders_WhenNoFilters()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer1 = await context.Customers.FirstAsync(c => c.CustomerId == 1);
        var customer2 = await context.Customers.FirstAsync(c => c.CustomerId == 2);

        var orders = new List<Order>
        {
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'P', Amount = 1000, Date = DateTime.Now },
            new Order { CustomerId = customer2.CustomerId, CustomerName = customer2.CustomerName, StatusCode = 'S', Amount = 2000, Date = DateTime.Now },
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'C', Amount = 3000, Date = DateTime.Now }
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllOrdersAsync();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldFilterByStatus_WhenStatusCodeProvided()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer = await context.Customers.FirstAsync();

        var orders = new List<Order>
        {
            new Order { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, StatusCode = 'P', Amount = 1000, Date = DateTime.Now },
            new Order { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, StatusCode = 'P', Amount = 2000, Date = DateTime.Now },
            new Order { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, StatusCode = 'S', Amount = 3000, Date = DateTime.Now },
            new Order { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName, StatusCode = 'C', Amount = 4000, Date = DateTime.Now }
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllOrdersAsync('P');

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, o => Assert.Equal('P', o.StatusCode));
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldFilterByCustomer_WhenCustomerIdProvided()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer1 = await context.Customers.FirstAsync(c => c.CustomerId == 1);
        var customer2 = await context.Customers.FirstAsync(c => c.CustomerId == 2);

        var orders = new List<Order>
        {
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'P', Amount = 1000, Date = DateTime.Now },
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'S', Amount = 2000, Date = DateTime.Now },
            new Order { CustomerId = customer2.CustomerId, CustomerName = customer2.CustomerName, StatusCode = 'P', Amount = 3000, Date = DateTime.Now }
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllOrdersAsync(null, customer1.CustomerId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, o => Assert.Equal(customer1.CustomerId, o.CustomerId));
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldFilterByBothStatusAndCustomer_WhenBothProvided()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer1 = await context.Customers.FirstAsync(c => c.CustomerId == 1);
        var customer2 = await context.Customers.FirstAsync(c => c.CustomerId == 2);

        var orders = new List<Order>
        {
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'P', Amount = 1000, Date = DateTime.Now },
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'S', Amount = 2000, Date = DateTime.Now },
            new Order { CustomerId = customer2.CustomerId, CustomerName = customer2.CustomerName, StatusCode = 'P', Amount = 3000, Date = DateTime.Now },
            new Order { CustomerId = customer1.CustomerId, CustomerName = customer1.CustomerName, StatusCode = 'P', Amount = 4000, Date = DateTime.Now }
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllOrdersAsync('P', customer1.CustomerId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, o =>
        {
            Assert.Equal('P', o.StatusCode);
            Assert.Equal(customer1.CustomerId, o.CustomerId);
        });
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldRemoveOrderFromDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new OrderServise(context);
        var customer = await context.Customers.FirstAsync();

        var order = new Order
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            StatusCode = 'P',
            Amount = 1500,
            Date = DateTime.Now
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();
        var orderId = order.OrderId;

        // Act
        await service.DeleteOrderAsync(orderId);

        // Assert
        var deletedOrder = await context.Orders.FindAsync(orderId);
        Assert.Null(deletedOrder);
    }
}

