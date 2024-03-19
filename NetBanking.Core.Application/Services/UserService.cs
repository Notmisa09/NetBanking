using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public UserService(IMapper mapper, IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginrequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginrequest);
            return userResponse;
        }

        public async Task SingOutAsync()
        {
            await _accountService.SingOutAsync();
        }

        public async Task<ServiceResult> RegisterAsync(SaveUserViewModel vm, string origin, string userRole)
        {
            RegisterRequest resgisterRequest = _mapper.Map<RegisterRequest>(vm);
            if(resgisterRequest !=  null && string.IsNullOrEmpty(resgisterRequest.Id))
            {
                if(vm.formFile  != null)
                {
                    resgisterRequest.ImageURL = UploadImage.UploadFile(vm.formFile, resgisterRequest.Id, "User");
                }
            }
            return await _accountService.RegisterUserAsync(resgisterRequest, origin, userRole);
        }

        public async Task<string> ConfirmEmailAsync(string UserId, string token)
        {
            return await _accountService.ConfirmAccountAsync(UserId, token);
        }

        public async Task<ServiceResult> ForgotPasswordAsync(ForgorPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPassswordAsync(forgotRequest, origin);
        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }

        public async Task<UserViewModel> GetByIdAsync(string UserId)
        {
            var user = await _accountService.GetByIdAsync(UserId);
            UserViewModel vm = _mapper.Map<UserViewModel>(user);
            return vm;
        }
    }
}
