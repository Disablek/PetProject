using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Business.Interfaces;
using TaskFlow.Business.Repositories;
using TaskFlow.Business.Services;
using TaskFlow.Data;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<TaskFlowDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskFlowDbContext))); 
});

// AddAsync services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DI
builder.Services.AddScoped<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// MW
app.UseCustomMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
