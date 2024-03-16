using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;

namespace NetBanking.Core.Application.Interfaces.IServices
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<ServiceResult> RegisterUserAsync(RegisterRequest request, string origin, string UserRoles);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ServiceResult> ForgotPassswordAsync(ForgotPasswordRequest request, string origin);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task SingOutAsync();
    }
}
