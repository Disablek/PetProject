using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Data.Services;

public class DataSeeder : IDataSeeder
{
    private readonly TaskFlowDbContext _context;

    public DataSeeder(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Проверяем, есть ли уже данные в базе
        if (await _context.Users.AnyAsync())
        {
            return; // Данные уже есть, пропускаем заполнение
        }

        // Создаем тестовых пользователей
        var users = new List<UserEntity>
        {
            new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@taskflow.com",
                FullName = "Администратор Системы",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "john_doe",
                Email = "john@taskflow.com",
                FullName = "Джон Доу",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "jane_smith",
                Email = "jane@taskflow.com",
                FullName = "Джейн Смит",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "mike_wilson",
                Email = "mike@taskflow.com",
                FullName = "Майк Уилсон",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
            }
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Создаем тестовые проекты
        var projects = new List<ProjectEntity>
        {
            new ProjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Веб-приложение TaskFlow",
                Description = "Разработка системы управления задачами",
                AdminId = users[0].Id
            },
            new ProjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "Мобильное приложение",
                Description = "Создание мобильной версии приложения",
                AdminId = users[1].Id
            },
            new ProjectEntity
            {
                Id = Guid.NewGuid(),
                Name = "API документация",
                Description = "Создание и поддержка API документации",
                AdminId = users[2].Id
            }
        };

        await _context.Projects.AddRangeAsync(projects);
        await _context.SaveChangesAsync();

        // Добавляем пользователей к проектам (many-to-many)
        projects[0].Users.Add(users[0]);
        projects[0].Users.Add(users[1]);
        projects[0].Users.Add(users[2]);
        
        projects[1].Users.Add(users[1]);
        projects[1].Users.Add(users[3]);
        
        projects[2].Users.Add(users[2]);
        projects[2].Users.Add(users[3]);
        
        await _context.SaveChangesAsync();

        // Создаем тестовые задачи
        var tasks = new List<TaskEntity>
        {
            // Задачи для первого проекта
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Настройка базы данных",
                Description = "Настроить подключение к PostgreSQL и создать миграции",
                Status = Status.Done,
                Priority = Priority.High,
                ProjectId = projects[0].Id,
                CreatorId = users[0].Id,
                AssigneeId = users[1].Id,
                DueTime = DateTime.UtcNow.AddDays(-5),
                CompletedTime = DateTime.UtcNow.AddDays(-3)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Создание API контроллеров",
                Description = "Реализовать CRUD операции для задач через API",
                Status = Status.InProgress,
                Priority = Priority.High,
                ProjectId = projects[0].Id,
                CreatorId = users[0].Id,
                AssigneeId = users[2].Id,
                DueTime = DateTime.UtcNow.AddDays(3)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Написание тестов",
                Description = "Создать unit и integration тесты для сервисов",
                Status = Status.New,
                Priority = Priority.Medium,
                ProjectId = projects[0].Id,
                CreatorId = users[1].Id,
                AssigneeId = users[3].Id,
                DueTime = DateTime.UtcNow.AddDays(7)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Документация API",
                Description = "Создать Swagger документацию для всех endpoints",
                Status = Status.Review,
                Priority = Priority.Medium,
                ProjectId = projects[0].Id,
                CreatorId = users[2].Id,
                AssigneeId = users[0].Id,
                DueTime = DateTime.UtcNow.AddDays(2)
            },

            // Задачи для второго проекта
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Дизайн мобильного интерфейса",
                Description = "Создать макеты экранов для мобильного приложения",
                Status = Status.InProgress,
                Priority = Priority.High,
                ProjectId = projects[1].Id,
                CreatorId = users[1].Id,
                AssigneeId = users[3].Id,
                DueTime = DateTime.UtcNow.AddDays(5)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Настройка React Native",
                Description = "Настроить окружение разработки для React Native",
                Status = Status.New,
                Priority = Priority.Medium,
                ProjectId = projects[1].Id,
                CreatorId = users[1].Id,
                AssigneeId = users[3].Id,
                DueTime = DateTime.UtcNow.AddDays(10)
            },

            // Задачи для третьего проекта
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Создание OpenAPI спецификации",
                Description = "Создать детальную OpenAPI спецификацию для всех endpoints",
                Status = Status.InProgress,
                Priority = Priority.High,
                ProjectId = projects[2].Id,
                CreatorId = users[2].Id,
                AssigneeId = users[2].Id,
                DueTime = DateTime.UtcNow.AddDays(4)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Примеры использования API",
                Description = "Создать примеры кода для интеграции с API",
                Status = Status.New,
                Priority = Priority.Low,
                ProjectId = projects[2].Id,
                CreatorId = users[2].Id,
                AssigneeId = users[3].Id,
                DueTime = DateTime.UtcNow.AddDays(14)
            },
            new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Обновление документации",
                Description = "Обновить README и добавить инструкции по развертыванию",
                Status = Status.Blocked,
                Priority = Priority.Medium,
                ProjectId = projects[2].Id,
                CreatorId = users[3].Id,
                AssigneeId = users[2].Id,
                DueTime = DateTime.UtcNow.AddDays(6)
            }
        };

        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        Console.WriteLine("Тестовые данные успешно добавлены в базу данных!");
    }
}
