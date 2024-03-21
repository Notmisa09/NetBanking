using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class SavingsAccountService : GenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>, ISavingsAccountService
    {
        private readonly IMapper _mapper;
        private readonly ISavingsAccountRepository _repository;
        private readonly IAccountService _accountService;
        public SavingsAccountService(
            IAccountService accountService,
            IMapper mapper,
            ISavingsAccountRepository repository) : base(repository, mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<SavingsAccountViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<SavingsAccountViewModel>>(list);
        }

        public async Task SaveUserWIthMainAccount(SaveUserViewModel vm)
        {
            string productcode = string.Empty;
            var userinfo = await _accountService.GetByEmail(vm.Email);

            SavingsAccount savingAccount = new()
            {
                Amount = vm.InitialAmount,
                IsMain = true,
                UserId = userinfo.Id,
                CreatedDate = DateTime.Now,
                CreatedById = "Default",
                Id = "19819191"
            };
            await _repository.AddAsync(savingAccount);
        }
    }
}
