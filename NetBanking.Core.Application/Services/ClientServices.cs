using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.ViewModels.Client;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Transaction;

namespace NetBanking.Core.Application.Services
{
    public class ClientServices
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ITransactionService _transactionService;

        public ClientServices(
            IAccountService accountService, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ISavingsAccountService savingsAccountService,
            ICreditCardService creditCardService,
            ILoanService loanService,
            IBeneficiaryService beneficiaryService,
            ITransactionService transactionService
            )
        {
            _accountService = accountService;
            _beneficiaryService = beneficiaryService;
            _creditCardService = creditCardService;
            _transactionService = transactionService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loanService = loanService;
            _savingsAccountService = savingsAccountService;
        }
        
        public async Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            GetAllProductsByClientViewModel vm = new()
            {
                SavingsAccounts = await _savingsAccountService.GetByOwnerIdAsync(userId),
                CreditCards = await _creditCardService.GetByOwnerIdAsync(userId),
                Loans = await _loanService.GetByOwnerIdAsync(userId)
            };
            
            return vm;
        }

        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiariesByClientAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            return await _beneficiaryService.GetByOwnerIdAsync(userId);
        }

        public async Task RealizeTransaction(SaveTransactionViewModel svm)
        {
            if(svm.Type == Domain.Enums.TransactionType.ExpressPay)
            {
                svm.EmissorProductId = "1";
            }
            await _transactionService.AddAsync(svm);
        }

        public async Task<dynamic> GetProductByIdAsync(string Id)
        {
            if (100 >= Convert.ToInt32(Id.Substring(0,3)) && Convert.ToInt32(Id.Substring(0, 3)) <=300 )
            {

            }
        }

        //Falta hacer la interfaz
    }
}
