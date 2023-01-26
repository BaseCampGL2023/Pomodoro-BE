using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Core.Interfaces;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Api.Controllers;

public class TasksController : BaseController
{
    private readonly ITaskRepository _tasksRepo;

    public TasksController(ITaskRepository tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskEntity>>> GetTasks()
    {
        var tasks = await this._tasksRepo.ListAllAsync();
        return this.Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntity>> GetTask(int id)
    {
        var task = await this._tasksRepo.GetByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        return this.Ok(task);
    }

    /*[HttpPost]
    public async Task<ActionResult<TaskEntity>> PostTask(Task)
    {

    }*/

}
