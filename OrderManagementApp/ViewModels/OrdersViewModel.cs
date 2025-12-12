using Microsoft.AspNetCore.Mvc.Rendering;
using OrderManagementApp.Data.DTO;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.ViewModels;

public class OrdersViewModel
{
    public List<Order> Orders { get; set; }
    public OrderSummaryDto Summary { get; set; }
    
    // Filter selection values
    public char? SelectedStatus { get; set; }
    public int? SelectedCustomer { get; set; }

    // Dropdown lists
    public SelectList CustomerList { get; set; }
    public SelectList StatusList { get; set; }
}