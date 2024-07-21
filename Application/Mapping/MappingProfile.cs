using AutoMapper;
using Core.Models;
using Application.DTOs;
using Task = Core.Models.Task;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Task, TaskDto>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<TaskCreateDto, Task>().ReverseMap();
        }
    }
}
