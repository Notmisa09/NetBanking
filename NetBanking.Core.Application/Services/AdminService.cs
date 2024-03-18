using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Dashboard;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Enums;

namespace NetBanking.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _trasactionRepository;
        public AdminService(IAccountService accountService, IMapper mapper, ITransactionRepository trasactionRepository)
        {
            _accountService = accountService;
            _mapper = mapper;
            _trasactionRepository = trasactionRepository;
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var user = await _accountService.GetAllUsers();
            var userlist = _mapper.Map<List<UserViewModel>>(user);
            return userlist;
        }

        public async Task<DashboardViewModel> GetDashboard()
        {
            DashboardViewModel vm = new DashboardViewModel();
            var transactions = await _trasactionRepository.GetAllAsync();

            vm.AllPaymentsNumber = transactions.GroupBy(x => x.Type).Count();
            vm.AllTransaction = transactions.Count();
            vm.AllPayments = transactions.GroupBy(x => x.Cantity).Sum(group => group.Count());

            //Me falta completar el dashboard - Yahinniel. 
            return vm;
        }
    }
}
