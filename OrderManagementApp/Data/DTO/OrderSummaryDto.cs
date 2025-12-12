namespace OrderManagementApp.Data.DTO;

public class OrderSummaryDto
{
    public decimal TotalTurnover { get; set; }
    public decimal AverageOrderValue { get; set; }
    public string TopCustomer { get; set; }
}