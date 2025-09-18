# TaskFlow - Система управления задачами

## Описание

TaskFlow - это система управления задачами, построенная на архитектуре Clean Architecture с использованием .NET 8, Entity Framework Core и PostgreSQL.

## Структура проекта

- **API** - Web API слой с контроллерами и middleware
- **Application** - Бизнес-логика, сервисы и DTO
- **Infrastructure** - Доступ к данным, репозитории и Entity Framework
- **Domain** - Доменные модели и интерфейсы

## Запуск проекта

### 1. Запуск базы данных

Используйте Docker Compose для запуска PostgreSQL и pgAdmin:

```bash
docker-compose up -d
```

Это запустит:
- PostgreSQL на порту 5432
- pgAdmin на порту 5050 (http://localhost:5050)

### 2. Настройка подключения

Подключение к базе данных уже настроено в `API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "TaskFlowDbContext": "User ID=myuser;Password=mypassword;Host=localhost;Port=5432;Database=mydatabase;"
  }
}
```

### 3. Применение миграций

```bash
cd API
dotnet ef database update
```

### 4. Запуск приложения

```bash
cd API
dotnet run
```

API будет доступен по адресу: https://localhost:7000

## Тестовые данные

При первом запуске приложения автоматически заполняются тестовые данные:

### Пользователи
- **admin** (admin@taskflow.com) - Администратор Системы
- **john_doe** (john@taskflow.com) - Джон Доу  
- **jane_smith** (jane@taskflow.com) - Джейн Смит
- **mike_wilson** (mike@taskflow.com) - Майк Уилсон

Пароль для всех пользователей: `password123` (для admin: `admin123`)

### Проекты
1. **Веб-приложение TaskFlow** - Разработка системы управления задачами
2. **Мобильное приложение** - Создание мобильной версии приложения  
3. **API документация** - Создание и поддержка API документации

### Задачи
Создано 9 тестовых задач с различными статусами:
- Новые задачи
- Задачи в работе
- Завершенные задачи
- Задачи на проверке
- Заблокированные задачи

## API Endpoints

### Задачи
- `GET /api/tasks` - Получить все задачи
- `GET /api/tasks/{id}` - Получить задачу по ID
- `GET /api/tasks/project/{projectId}` - Получить задачи по проекту
- `POST /api/tasks` - Создать новую задачу
- `PUT /api/tasks/{id}` - Обновить задачу
- `DELETE /api/tasks/{id}` - Удалить задачу

## Swagger UI

После запуска приложения Swagger UI доступен по адресу:
https://localhost:7000/swagger

## pgAdmin

Для управления базой данных используйте pgAdmin:
- URL: http://localhost:5050
- Email: admin@admin.com
- Password: admin

Подключение к базе данных в pgAdmin:
- Host: postgres_db (или localhost)
- Port: 5432
- Database: mydatabase
- Username: myuser
- Password: mypassword
