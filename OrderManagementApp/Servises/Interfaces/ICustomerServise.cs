using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public interface ICustomerServise
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(int id);
    Task CreateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
}