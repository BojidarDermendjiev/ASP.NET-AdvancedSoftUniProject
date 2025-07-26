namespace ServerAspNetCoreAPIMakePC.Application.Mappings
{
    using AutoMapper;
    
    using DTOs;
    using Utilities;
    using Domain.Entities;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .AfterMap((src, dest) =>
                {
                    byte[] salt;
                    dest.PasswordHash = PasswordHasher.HashPassword(src.Password, out salt);
                    dest.PasswordSalt = Convert.ToHexString(salt);
                });
        }
    }
}
