using AutoMapper;
using userService.Domain.Entities;

namespace userService.Api.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    { 
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, UserLoginDTO>().ReverseMap();
    }
}