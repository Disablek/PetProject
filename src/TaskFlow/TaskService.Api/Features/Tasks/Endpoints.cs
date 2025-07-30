using Microsoft.AspNetCore.Builder;

namespace TaskService.Api.Features.Tasks
{
    public static class Endpoints
    {
        public static void MapTaskEndpoints(this WebApplication app)
        {
            app.MapGet("/tasks", (InMemoryTaskRepository repo) =>
                Results.Ok(repo.GetAll()));

            app.MapGet("/tasks/{id:guid}", (InMemoryTaskRepository repo, Guid id) =>
            {
                var task = repo.GetById(id);
                return task is not null ? Results.Ok(task) : Results.NotFound();
            });

            app.MapPost("/tasks", (InMemoryTaskRepository repo, TaskItem task) =>
            {
                repo.Add(task);
                return Results.Created($"/tasks/{task.Id}", task);
            });

            app.MapPut("/tasks/{id:guid}", (InMemoryTaskRepository repo, Guid id, TaskItem updated) =>
            {
                updated.Id = id;
                return repo.Update(updated) ? Results.NoContent() : Results.NotFound();
            });

            app.MapDelete("/tasks/{id:guid}", (InMemoryTaskRepository repo, Guid id) =>
                repo.Delete(id) ? Results.NoContent() : Results.NotFound());
        }
    }
}
