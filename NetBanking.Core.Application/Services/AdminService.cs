using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Dashboard;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _trasactionRepository;
        private readonly AuthenticationResponse _userViewModel;

        //Transacciones
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        private readonly ISavingsAccountService _savingsAccountService;

        public AdminService(IAccountService accountService, IMapper mapper,
            ITransactionRepository trasactionRepository, ICreditCardRepository creditCardRepository,
            ILoanRepository loanRepository, ISavingsAccountRepository savingsAccountRepository,
            IHttpContextAccessor httpContextAccessor, ISavingsAccountService savingsAccountService)
        {
            _savingsAccountService = savingsAccountService;
            _accountService = accountService;
            _mapper = mapper;
            _userViewModel = httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _creditCardRepository = creditCardRepository;
            _loanRepository = loanRepository;
            _savingsAccountRepository = savingsAccountRepository;
            _trasactionRepository = trasactionRepository;
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var user = await _accountService.GetAllUsers();
            user.Where(x => x.Id != _userViewModel.Id).ToList();
            var userlist = _mapper.Map<List<UserViewModel>>(user);
            return userlist;
        }

        public async Task<string> ChangeAccountStatus(string Id)
        {
            if (Id == _userViewModel.Id)
            {
                return "No puedes desactivar tu propia cuenta.";
            }
            else
            {
                var user = await _accountService.GetByIdAsync(Id);
                if (user != null)
                {
                    if(user.IsActive == true)
                    {
                        user.IsActive = false;
                    }
                    else
                    {
                        user.IsActive = true;
                    }
                    var userVm = _mapper.Map<RegisterRequest>(user);
                    await _accountService.ChangeUserStatus(userVm);

                    return "Se ha cambiado el estado de la cuenta";
                }
                else
                {
                    return "No se encontro el usuario.";
                }
            }
        }

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
            vmDashBoard.ActiveClients = user.Where(x => x.Roles.Contains("Client") && x.IsActive == true).Count();
            vmDashBoard.InactiveClients = user.Where(x => x.Roles.Contains("Client") && x.IsActive == false).Count();
            vmDashBoard.AssignedProduct = totalCount;

            return vmDashBoard;
        }

        //public async Task<SaveUserViewModel> GetByIdWithAmountAsync(string UserId)
        //{
        //    var user = await _accountService.GetByIdAsync(UserId);
        //    //var savingsAccount = await _savingsAccountService.GetByOwnerIdAsync(user.Id);
        //    //var savingsAccountVm = savingsAccount.Find(x => x.IsMain == true && x.UserId == UserId);
        //    SaveUserViewModel vm = _mapper.Map<SaveUserViewModel>(user);
        //    vm.InitialAmount = 0;
        //    return vm;
        //}
    }
}
