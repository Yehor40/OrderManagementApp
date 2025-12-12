using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Data;
using OrderManagementApp.Data.Entities;

namespace OrderManagementApp.Servises;

public class StatusServise:IStatusServise
{
    private readonly AppDbContext _context;

    public StatusServise(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Status>> GetAllStatusesAsync()
    {
        return await _context.Statuses
            .OrderBy(s => s.StatusName)
            .ToListAsync();
    }

    public async Task<Status> GetStatusByCodeAsync(char code)
    {
        return await _context.Statuses.FindAsync(code) ?? throw new InvalidOperationException();
    }
}