using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationResponse, SaveUserViewModel>()
               .ForMember(c => c.ConfirmPassword, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(c => c.IsVerified, opt => opt.Ignore())
               .ForMember(c => c.UserStatus, opt => opt.Ignore())
               .ForMember(c => c.Roles, opt => opt.Ignore());

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(r => r.Error, opt => opt.Ignore())
                .ForMember(r => r.HasError, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(r => r.UserStatus, opt => opt.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(a => a.Error , opt => opt.Ignore())
                .ForMember(a => a.HasError , opt => opt.Ignore())   
                .ReverseMap();
        }
    }
}
