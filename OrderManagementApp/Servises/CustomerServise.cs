using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Data;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public class CustomerServise: ICustomerServise
{
    private readonly AppDbContext _context;

    public CustomerServise(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.CustomerName)
            .ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task CreateCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}