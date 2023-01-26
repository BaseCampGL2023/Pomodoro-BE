
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
        await _context.Tasks.AddAsync(item);
        return item;
    }


    public Task DeleteAsync(TaskEntity item)
    {
        _context.Tasks.Remove(item);
        return Task.CompletedTask;
    }

    public async Task<TaskEntity> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Include(t => t.Frequency)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IReadOnlyList<TaskEntity>> ListAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public Task UpdateAsync(TaskEntity item)
    {
        _context.Tasks.Update(item);
        return Task.CompletedTask;
    }
}