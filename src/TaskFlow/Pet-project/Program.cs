using Pet_project;

class Program
{
    static void Main()
    {
        var service = new TaskService();
        service.TasksChanged += () => Console.WriteLine("\n[!] Список задач изменён.\n");

        while (true)
        {
            Console.WriteLine("--- Меню ---");
            Console.WriteLine("1. Добавить задачу");
            Console.WriteLine("2. Показать все задачи");
            Console.WriteLine("3. Удалить задачу");
            Console.WriteLine("4. Отметить задачу как выполненную");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    Console.Write("Введите название задачи: ");
                    var title = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(title))
                        service.AddNewTask(title);
                    else
                        Console.WriteLine("Название не может быть пустым!");
                    break;
                case "2":
                    var tasks = service.GetAll();
                    if (tasks.Count == 0)
                        Console.WriteLine("Список задач пуст.\n");
                    else
                        foreach (var t in tasks)
                            Console.WriteLine(t);
                    break;
                case "3":
                    Console.Write("Введите Id задачи для удаления: ");
                    if (Guid.TryParse(Console.ReadLine(), out var removeId))
                    {
                        if (!service.RemoveTask(removeId))
                            Console.WriteLine("Задача не найдена!\n");
                    }
                    else
                        Console.WriteLine("Некорректный Id!\n");
                    break;
                case "4":
                    Console.Write("Введите Id задачи для отметки как выполненной: ");
                    if (Guid.TryParse(Console.ReadLine(), out var completeId))
                    {
                        if (!service.CompleteTask(completeId))
                            Console.WriteLine("Задача не найдена или уже выполнена!\n");
                    }
                    else
                        Console.WriteLine("Некорректный Id!\n");
                    break;
                case "0":
                    Console.WriteLine("Выход...");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда!\n");
                    break;
            }
        }
    }
}
