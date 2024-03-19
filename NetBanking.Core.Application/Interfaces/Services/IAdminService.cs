using NetBanking.Core.Application.ViewModels.Dashboard;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetAllAsync();
        Task<DashboardViewModel> GetDashboard();
    }
}
