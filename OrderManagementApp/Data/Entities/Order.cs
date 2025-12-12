using System.ComponentModel.DataAnnotations;

namespace OrderManagementApp.Data.Entities;

public class Order
{
    // PK
    [Key]
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    // FK: CustomerID 
    public int CustomerId { get; set; }
    // Navigation Property(Order->Customer M:1) )
    public virtual Customer Customer { get; set; }
    // FK: StatusCode
    public char StatusCode { get; set; }
    // Navigation Property(Order->Status 1:1)
    public virtual Status Status { get; set; }
}