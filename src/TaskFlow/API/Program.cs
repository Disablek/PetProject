using API.Middleware;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Business.DTO.Task.Mapper;            // <-- если здесь лежат профили
using TaskFlow.Business.Services;           // <-- твои сервисы/интерфейсы
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data;
using TaskFlow.Data.Repositories;
using TaskFlow.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// DbContext
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskFlowDbContext)));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- AutoMapper: регистрация профилей ----------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// ---------- Регистрация репозиториев и BLL-сервисов ----------
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Зарегистрируй остальные репозитории/сервисы аналогично:
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
// builder.Services.AddScoped<IProjectService, ProjectService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomMiddleware();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
