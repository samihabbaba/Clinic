using AutoMapper;
using Clinic.API.Dtos.SystemUsersDto;
using Clinic.API.Dtos.UserListDto;
using Clinic.API.Dtos.UserUpdateDto;
using Clinic.API.Models;

namespace Clinic.API.MappingProfile
{
    public class DomainToResponseProfile: Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Dtos.UserCreationDto.SystemUserCreationDto, SystemUser>();
            CreateMap<UpdateDto, SystemUser>();
            CreateMap<SystemUser, SystemUserViewDto>();
            CreateMap<RegisterDto, SystemUser>();
        }
    }
}