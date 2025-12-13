using OrderManagementApp.Data.Entities;
using OrderManagementApp.Servises;
using OrderManagementApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderManagementApp.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderServise _orderService;
    private readonly ICustomerServise _customerService;
    private readonly IStatusServise _statusService;

    public OrdersController(IOrderServise orderService, ICustomerServise customerService, IStatusServise statusService)
    {
        _orderService = orderService;
        _customerService = customerService;
        _statusService = statusService;
    }

    // GET: /Orders/Index
    public async Task<IActionResult> Index(char? selectedStatus, int? selectedCustomer)
    {
        var customers = await _customerService.GetAllCustomersAsync();
        var statuses = await _statusService.GetAllStatusesAsync();

        var viewModel = new OrdersViewModel
        {
            Orders = await _orderService.GetAllOrdersAsync(selectedStatus, selectedCustomer),
            Summary = await _orderService.GetOrderSummaryAsync(),
            SelectedStatus = selectedStatus,
            SelectedCustomer = selectedCustomer,
            CustomerList = new SelectList(customers, nameof(Customer.CustomerId), nameof(Customer.CustomerName),selectedCustomer),
            StatusList = new SelectList(statuses, nameof(Status.Code), nameof(Status.StatusName),selectedStatus)
        };

        return View(viewModel);
    }

    // GET: Orders/Create
    public async Task<IActionResult> Create()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        var statuses = await _statusService.GetAllStatusesAsync();
        var model = new CreateOrderViewModel
        {
            CustomerList = new SelectList(customers, nameof(Customer.CustomerId), nameof(Customer.CustomerName)),
            StatusList = new SelectList(statuses, nameof(Status.Code), nameof(Status.StatusName))
        };
        return View(model);
    }

// POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = await _customerService.GetCustomerByIdAsync(model.SelectedCustomerId);

            var newOrder = new Order
            {
                CustomerId = model.SelectedCustomerId,
                CustomerName = customer?.CustomerName ?? "Unknown", // Filling your required attribute
                StatusCode = model.SelectedStatusCode,
                Amount = model.Amount,
                Date = DateTime.Now
            };

            await _orderService.CreateOrderAsync(newOrder);
            return RedirectToAction(nameof(Index));
        }

        model.CustomerList = new SelectList(await _customerService.GetAllCustomersAsync(), nameof(Customer.CustomerId),nameof(Customer.CustomerName));
        model.StatusList = new SelectList(await _statusService.GetAllStatusesAsync(), nameof(Status.Code), nameof(Status.StatusName));
        return View(model);
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _orderService.GetOrderByIdAsync(id.Value);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _orderService.DeleteOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }
}