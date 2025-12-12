using OrderManagementApp.Data.DTO;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public interface IOrderServise
{
    Task<List<Order>> GetAllOrdersAsync(char? statusCode = null, int? customerId = null);
    Task<Order?> GetOrderByIdAsync(int id);
    Task<OrderSummaryDto> GetOrderSummaryAsync();
    Task CreateOrderAsync(Order order);
    Task DeleteOrderAsync(int id);
}