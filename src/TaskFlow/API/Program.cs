using API.Middleware;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Business.Extensions;
using TaskFlow.Data;
using TaskFlow.Data.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// DbContext
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskFlowDbContext)));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

// ---------- CORS: настройка для фронтенда ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",    // Vite dev server
                "http://localhost:5173",    // Альтернативный порт Vite
                "http://127.0.0.1:3000",   // Альтернативный localhost
                "http://127.0.0.1:5173"    // Альтернативный localhost
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ---------- AutoMapper: регистрация профилей ----------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ---------- Регистрация Business сервисов ----------
builder.Services.AddBusinessServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------- CORS: включение политики ----------
app.UseCors("AllowFrontend");

app.UseCustomMiddleware();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Заполнение базы данных тестовыми данными
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}

app.Run();
