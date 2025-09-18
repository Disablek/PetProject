using AutoMapper;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.User;
using TaskFlow.Data.Entities;

namespace TaskFlow.Business;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Task mappings
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(d => d.AssigneeId, opt => opt.MapFrom(s => s.AssigneeId))
            .ForMember(d => d.CreatorId, opt => opt.MapFrom(s => s.CreatorId))
            .ForMember(d => d.ProjectId, opt => opt.MapFrom(s => s.ProjectId));

        CreateMap<TaskDto, TaskEntity>()
            .ForMember(e => e.AssigneeId, opt => opt.MapFrom(d => d.AssigneeId))
            .ForMember(e => e.CreatorId, opt => opt.MapFrom(d => d.CreatorId))
            .ForMember(e => e.ProjectId, opt => opt.MapFrom(d => d.ProjectId))
            .ForMember(e => e.Assignee, opt => opt.Ignore())
            .ForMember(e => e.Creator, opt => opt.Ignore())
            .ForMember(e => e.Project, opt => opt.Ignore());

        CreateMap<CreateTaskDto, TaskEntity>()
            .ForMember(e => e.Id, opt => opt.Ignore())
            .ForMember(e => e.Status, opt => opt.Ignore())
            .ForMember(e => e.CompletedTime, opt => opt.Ignore())
            .ForMember(e => e.Assignee, opt => opt.Ignore())
            .ForMember(e => e.Creator, opt => opt.Ignore())
            .ForMember(e => e.Project, opt => opt.Ignore());

        CreateMap<UpdateTaskDto, TaskEntity>()
            .ForMember(e => e.Id, opt => opt.Ignore())
            .ForMember(e => e.CompletedTime, opt => opt.Ignore())
            .ForMember(e => e.CreatorId, opt => opt.Ignore())
            .ForMember(e => e.Assignee, opt => opt.Ignore())
            .ForMember(e => e.Creator, opt => opt.Ignore())
            .ForMember(e => e.Project, opt => opt.Ignore());

        CreateMap<TaskEntity, TaskListItemDto>()
            .ForMember(d => d.AssigneeName, opt => opt.MapFrom(s => s.Assignee != null ? s.Assignee.UserName : null));

        CreateMap<TaskEntity, TaskPreviewDto>();

        // User mappings
        CreateMap<UserEntity, UserDto>()
            .ForMember(d => d.Projects, opt => opt.MapFrom(s => s.Projects))
            .ForMember(d => d.CreatedTasks, opt => opt.MapFrom(s => s.CreatedTasks))
            .ForMember(d => d.AssignedTasks, opt => opt.MapFrom(s => s.AssignedTasks));

        CreateMap<UserDto, UserEntity>()
            .ForMember(e => e.Projects, opt => opt.MapFrom(d => d.Projects))
            .ForMember(e => e.CreatedTasks, opt => opt.MapFrom(d => d.CreatedTasks))
            .ForMember(e => e.AssignedTasks, opt => opt.MapFrom(d => d.AssignedTasks));

        CreateMap<CreateUserDto, UserEntity>()
            .ForMember(e => e.Projects, opt => opt.Ignore())
            .ForMember(e => e.CreatedTasks, opt => opt.Ignore())
            .ForMember(e => e.AssignedTasks, opt => opt.Ignore());

        CreateMap<UpdateUserDto, UserEntity>()
            .ForMember(e => e.Id, opt => opt.Ignore())
            .ForMember(e => e.UserName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.UserName)))
            .ForMember(e => e.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
            .ForMember(e => e.PasswordHash, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PasswordHash)))
            .ForMember(e => e.Projects, opt => opt.MapFrom(d => d.Projects))
            .ForMember(e => e.CreatedTasks, opt => opt.MapFrom(d => d.CreatedTasks))
            .ForMember(e => e.AssignedTasks, opt => opt.MapFrom(d => d.AssignedTasks));

        CreateMap<UserEntity, UserPreviewDto>();

        // Project mappings
        CreateMap<ProjectEntity, ProjectDto>()
            .ForMember(d => d.Tasks, opt => opt.MapFrom(s => s.Tasks))
            .ForMember(d => d.Users, opt => opt.MapFrom(s => s.Users))
            .ForMember(d => d.AdminId, opt => opt.MapFrom(s => s.AdminId));

        CreateMap<ProjectDto, ProjectEntity>()
            .ForMember(e => e.Tasks, opt => opt.MapFrom(d => d.Tasks))
            .ForMember(e => e.Users, opt => opt.MapFrom(d => d.Users))
            .ForMember(e => e.Admin, opt => opt.Ignore());

        CreateMap<CreateProjectDto, ProjectEntity>()
            .ForMember(e => e.Id, opt => opt.Ignore())
            .ForMember(e => e.Tasks, opt => opt.Ignore())
            .ForMember(e => e.Users, opt => opt.Ignore())
            .ForMember(e => e.Admin, opt => opt.Ignore());

        CreateMap<ProjectEntity, ProjcetListItemDto>()
            .ForMember(d => d.TasksCount, opt => opt.MapFrom(s => s.Tasks.Count));

        
        CreateMap<TaskDto, TaskPreviewDto>();
        CreateMap<UserDto, UserPreviewDto>();
    }
}
