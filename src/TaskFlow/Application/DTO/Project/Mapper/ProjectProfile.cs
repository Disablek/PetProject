using AutoMapper;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Data.Entities;

namespace TaskFlow.Business.DTO.Project.Mapper;
public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectDto, ProjectEntity>()
            .ForMember(p => p.AdminId, p => p
                .MapFrom(p => p.AdminId));

        CreateMap<ProjectEntity, ProjectDto>()
            .ForMember(p => p.AdminId, p => p
                .MapFrom(p => p.AdminId));

        CreateMap<UpdateTaskDto, TaskEntity>()
            .ForMember(e => e.Assignee, opt => opt.Ignore())
            .ForMember(e => e.Project, opt => opt.Ignore());

        CreateMap<TaskEntity, TaskListItemDto>();
    }
}
