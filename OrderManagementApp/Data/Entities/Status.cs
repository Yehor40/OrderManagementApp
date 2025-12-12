using System.ComponentModel.DataAnnotations;

namespace OrderManagementApp.Data.Entities;

public class Status
{
    //PK
    [Key]
    public char Code { get; set; }
    [MaxLength(50)]
    public string StatusName { get; set; }
    //Status -> Order (1:M)
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}