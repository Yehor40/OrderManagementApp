using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementApp.ViewModels;

public class CreateOrderViewModel
{
    [Required(ErrorMessage = "Please select a customer")]
    public int SelectedCustomerId { get; set; }

    [Required(ErrorMessage = "Please select a status")]
    public char SelectedStatusCode { get; set; }

    [Required]
    [Range(1, 1000000, ErrorMessage = "Amount must be greater than 0")]
    public int Amount { get; set; }

    // Dropdowns
    public SelectList? CustomerList { get; set; }
    public SelectList? StatusList { get; set; }
}