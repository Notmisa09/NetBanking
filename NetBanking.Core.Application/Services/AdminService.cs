using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Dashboard;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _trasactionRepository;

        //Transacciones
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ISavingsAccountRepository _savingsAccountRepository;


        public AdminService(IAccountService accountService, IMapper mapper,
            ITransactionRepository trasactionRepository, ICreditCardRepository creditCardRepository,
            ILoanRepository loanRepository, ISavingsAccountRepository savingsAccountRepository)
        {
            _accountService = accountService;
            _mapper = mapper;
            _creditCardRepository = creditCardRepository;
            _loanRepository = loanRepository;
            _savingsAccountRepository = savingsAccountRepository;
            _trasactionRepository = trasactionRepository;
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var user = await _accountService.GetAllUsers();
            var userlist = _mapper.Map<List<UserViewModel>>(user);
            return userlist;
        }

        public async Task

        public async Task<DashboardViewModel> GetDashboard()
        {
            DashboardViewModel vmDashBoard = new DashboardViewModel();

            var transactions = await _trasactionRepository.GetAllAsync();
            var user = await _accountService.GetAllUsers();

            var totalCount = await _creditCardRepository.GetAllAsync()
                .ContinueWith(creditCardTask => creditCardTask.Result.Count)
                .ContinueWith(creditCardCount => _loanRepository.GetAllAsync()
                    .ContinueWith(loanTask => loanTask.Result.Count + creditCardCount.Result))
                .Unwrap()
                .ContinueWith(totalCountTask => _savingsAccountRepository.GetAllAsync()
                    .ContinueWith(savingsTask => totalCountTask.Result + savingsTask.Result.Count))
                .Unwrap();

            vmDashBoard.AllTransaction = transactions.Count();
            vmDashBoard.AllPaymentsNumber = transactions.GroupBy(x => x.Type).Count();
            vmDashBoard.AllPayments = transactions.GroupBy(x => x.Cantity).Sum(group => group.Count());
            vmDashBoard.ActiveClients = user.GroupBy(x => x.IsActive == true).Count();
            vmDashBoard.InactiveClients = user.GroupBy(x => x.IsActive == false).Count();
            vmDashBoard.AssignedProduct = totalCount;

            return vmDashBoard;
        }
    }
}
