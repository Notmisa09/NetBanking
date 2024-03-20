using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services
{
    public class ClientServices
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _trasactionRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ISavingsAccountRepository _savingsAccountRepository;
        public ClientServices(IAccountService accountService, IMapper mapper, ITransactionRepository trasactionRepository, ICreditCardRepository creditCardRepository, ILoanRepository loanRepository, ISavingsAccountRepository savingsAccountRepository)
        {
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loanService = loanService;
            _savingsAccountService = savingsAccountService;
        }
        
        //public async Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync()
        //{
        //    var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
        //    GetAllProductsByClientViewModel vm = new()
        //    {
        //        SavingsAccounts = await _savingsAccountService.GetByOwnerIdAsync(userId),
        //        CreditCards = await _creditCardService.GetByOwnerIdAsync(userId),
        //        Loans = await _loanService.GetByOwnerIdAsync(userId)
        //    };
            
        //    return vm;
        //}

        //public async Task<List<BeneficiaryViewModel>> GetAllBeneficiariesByClientAsync()
        //{
        //    var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
        //    return await _beneficiaryService.GetByOwnerIdAsync(userId);
        //}

        //public async Task RealizeTransaction(SaveTransactionViewModel svm)
        //{
        //    if(svm.Type == Domain.Enums.TransactionType.ExpressPay)
        //    {
        //        svm.EmissorProductId = "1";
        //    }
        //    await _transactionService.AddAsync(svm);
        //}

        public async Task<List<UserViewModel>> GetAllClientProducts()
        {
            var user = await _accountService.GetAllUsers();
            var userlist = _mapper.Map<List<UserViewModel>>(user);
            return userlist;
        }

    }
}
