using AutoMapper;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.Services
{
    public class TaskService
    {
        private readonly ITasksRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITasksRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<List<TaskListItemDto>> GetAllAsync()
        {
            var entities = await _taskRepository.GetAllTasksAsync();
            if (entities == null || entities.Count == 0)
                return new List<TaskListItemDto>();

            return _mapper.Map<List<TaskListItemDto>>(entities);
        }
    }
}