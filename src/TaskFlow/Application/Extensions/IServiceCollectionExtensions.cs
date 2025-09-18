using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Business.Services;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data.Repositories;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Data.Services;

namespace TaskFlow.Business.Extensions;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы Business слоя
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов для цепочки вызовов</returns>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Регистрация репозиториев
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();

        // Регистрация бизнес-сервисов
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IUserService, UserService>();

        // Регистрация сервиса для заполнения тестовыми данными
        services.AddScoped<IDataSeeder, DataSeeder>();

        return services;
    }
}
