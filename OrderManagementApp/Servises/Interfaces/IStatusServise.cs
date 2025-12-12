using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public interface IStatusServise
{
    Task<List<Status>> GetAllStatusesAsync();
    Task<Status> GetStatusByCodeAsync(char code);
}