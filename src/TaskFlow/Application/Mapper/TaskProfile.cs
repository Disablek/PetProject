using AutoMapper;
using TaskFlow.Business.DTO;
using TaskFlow.Data.Entities;
namespace TaskFlow.Business.Mapper;
public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(d => d.AssigneeId,
                opt => opt
                    .MapFrom(s => s.AssigneeId == Guid.Empty ? null : (Guid?)s.AssigneeId));
        CreateMap<TaskDto, TaskEntity>()
            .ForMember(e => e.AssigneeId, opt => opt
                .MapFrom(d => d.AssigneeId ?? Guid.Empty));
    }
}
