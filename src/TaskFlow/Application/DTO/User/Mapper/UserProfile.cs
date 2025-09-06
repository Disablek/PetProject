using AutoMapper;
using TaskFlow.Data.Entities;

namespace TaskFlow.Business.DTO.User.Mapper;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserEntity>()
            .ForMember(u => u.PasswordHash, u => u
                .MapFrom(u => u.PasswordHash));

        CreateMap<UpdateUserDto, UserEntity>()
            .ForMember(u => u.AssignedTasks, u => u
                .MapFrom(u => u.AssignedTasks))
            .ForMember(u => u.CreatedTasks, u => u
                .MapFrom(u => u.CreatedTasks));
    }
}
