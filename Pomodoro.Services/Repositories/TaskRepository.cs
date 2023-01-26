
using Pomodoro.Core.Interfaces;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace Pomodoro.Services.Repositories;

public class TaskRepository : ITaskRepository
{

    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskEntity> CreateAsync(TaskEntity item)
    {
        var res = await _context.Tasks.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }


    public async Task<TaskEntity> DeleteAsync(TaskEntity item)
    {
        _context.Tasks.Remove(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetAllTasksByUserAsync(int userId)
    {
        return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<TaskEntity> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Include(t => t.Frequency)
            .FirstOrDefaultAsync(t => t.Id == id);
    }


    public async Task<TaskEntity> UpdateAsync(TaskEntity item)
    {
        _context.Tasks.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }
}