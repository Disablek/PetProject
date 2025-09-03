using AutoMapper;
using TaskFlow.Business.DTO;
using TaskFlow.Data.Entities;
namespace TaskFlow.Business.Mapper;
public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(d => d.AssigneeId, opt => opt
                .MapFrom(s => s.AssigneeId)) 
            .ForMember(d => d.CreatorId, opt => opt
                .MapFrom(s => s.CreatorId))
            .ForMember(d => d.ProjectId, opt => opt
                .MapFrom(s => s.ProjectId));

        CreateMap<TaskDto, TaskEntity>()
            .ForMember(e => e.AssigneeId, opt => opt
                .MapFrom(d => d.AssigneeId)) 
            .ForMember(e => e.CreatorId, opt => opt
                .MapFrom(d => d.CreatorId))
            .ForMember(e => e.ProjectId, opt => opt
                .MapFrom(d => d.ProjectId));
    }
}
