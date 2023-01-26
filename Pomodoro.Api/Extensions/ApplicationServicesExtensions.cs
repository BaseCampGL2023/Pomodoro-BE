using Pomodoro.Core.Interfaces;
using Pomodoro.Services.Repositories;

namespace Pomodoro.Api.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        return services;
    }
}
