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
            _trasactionRepository = trasactionRepository;
            _creditCardRepository = creditCardRepository;
            _loanRepository = loanRepository;
            _savingsAccountRepository = savingsAccountRepository;
        }

        public async Task<List<UserViewModel>> GetAllClientProducts()
        {
            var user = await _accountService.GetAllUsers();
            var userlist = _mapper.Map<List<UserViewModel>>(user);
            return userlist;
        }

    }
}
