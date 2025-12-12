namespace OrderManagementApp.Data.Entities;

public class Customer
{
    // PK:
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    // Customer -> Order (1:M)
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}