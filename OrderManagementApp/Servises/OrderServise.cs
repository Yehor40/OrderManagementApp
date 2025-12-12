using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Data;
using OrderManagementApp.Data.DTO;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public class OrderServise:IOrderServise
{
    private readonly AppDbContext _context;

    public OrderServise(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task <List<Order>> GetAllOrdersAsync(char? statusCode = null, int? customerId = null)
    {
        var query = _context.Orders
            .Include(o => o.Status)
            .Include(o => o.Customer)
            .AsQueryable();

        if (statusCode.HasValue && statusCode.Value != '0')
        {
            query = query.Where(o => o.StatusCode == statusCode.Value);
        }

        if (customerId.HasValue && customerId.Value != 0)
        {
            query = query.Where(o => o.CustomerId == customerId.Value);
        }

        return await query
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Status)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<OrderSummaryDto> GetOrderSummaryAsync()
    {
        var orders = await _context.Orders.ToListAsync();

        if (!orders.Any())
        {
            return new OrderSummaryDto { TopCustomer = "N/A" };
        }

        var total = orders.Sum(o => o.Amount);
        var avg = (decimal)orders.Average(o => o.Amount);

        // Calculate Customer with highest turnover using the CustomerName attribute in Order
        var topCustomer = orders
            .GroupBy(o => o.CustomerName)
            .Select(g => new { Name = g.Key, Total = g.Sum(o => o.Amount) })
            .OrderByDescending(x => x.Total)
            .FirstOrDefault()?.Name ?? "N/A";

        return new OrderSummaryDto
        {
            TotalTurnover = total,
            AverageOrderValue = avg,
            TopCustomer = topCustomer
        };
    }

    public async Task CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}