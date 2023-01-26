using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pomodoro.Core.Interfaces;
using Pomodoro.DataAccess.Entities;
using AutoMapper;
using Pomodoro.Api.ViewModels;

namespace Pomodoro.Api.Controllers;

/// <summary>
/// Presents the controller for tasks.
/// </summary>
public class TasksController : BaseController
{
    private readonly ITaskRepository tasksRepo;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksController"/> class.
    /// </summary>
    public TasksController(ITaskRepository tasksRepo, IMapper mapper)
    {
        this.tasksRepo = tasksRepo;
        this.mapper = mapper;
    }

    /// <summary>
    /// Endpoint to get all the tasks.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpGet("/user/{userId}")]
    public async Task<ActionResult<IReadOnlyList<TaskViewModel>>> GetTasks(int userId)
    {
        var tasks = await this.tasksRepo.GetAllTasksByUserAsync(userId);
        return this.Ok(this.mapper.Map<IReadOnlyList<TaskEntity>, IReadOnlyList<TaskViewModel>>(tasks));
    }

    /// <summary>
    /// Endpoint to get a specific task by its id.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<TaskViewModel>> GetTask(int id)
    {
        var task = await this.tasksRepo.GetByIdAsync(id);
        if (task == null)
        {
            return this.NotFound();
        }

        return this.Ok(this.mapper.Map<TaskEntity, TaskViewModel>(task));
    }

    /// <summary>
    /// Endpoint to create a task.
    /// </summary>
    /// <param name="task">Gets as a parameter Task view model</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost]
    public async Task<ActionResult<TaskViewModel>> PostTask(TaskViewModel task)
    {
        var data = this.mapper.Map<TaskViewModel, TaskEntity>(task);
        var result = await this.tasksRepo.CreateAsync(data);
        return this.Ok(this.mapper.Map<TaskEntity, TaskViewModel>(result));
    }

    /// <summary>
    /// Endpoint to delete a task.
    /// </summary>
    /// <param name="task">Gets as a parameter Task view model.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpDelete]
    public async Task<ActionResult> DeleteTask(TaskViewModel task)
    {
        var data = this.mapper.Map<TaskViewModel, TaskEntity>(task);
        await this.tasksRepo.DeleteAsync(data);
        return this.Ok();
    }

    /// <summary>
    /// Endpoint to update a task.
    /// </summary>
    /// <param name="task">Gets as a parameter Task view model.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPut]
    public async Task<ActionResult> UpdateTask(TaskViewModel task)
    {
        var data = this.mapper.Map<TaskViewModel, TaskEntity>(task);
        await this.tasksRepo.UpdateAsync(data);
        return this.Ok();
    }
}
