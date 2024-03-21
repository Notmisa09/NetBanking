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

        public SavingsAccountService(
            
            IMapper mapper,
            ISavingsAccountRepository repository) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<SavingsAccountViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<SavingsAccountViewModel>>(list);
        }

        /*public async Task SaveUserWIthMainAccount(SaveUserViewModel vm)
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
        }*/

        /*public async Task<string> Delete(string Id)
        {
            var savingsAccount = await _repository.GeEntityByIDAsync(Id);

            if (savingsAccount.IsMain == true)
            {
                return "La cuenta principal no puede ser eliminada.";
            }

            else if (savingsAccount.IsMain == false && savingsAccount.Amount >= 0)
            {
                var user = await _accountService.GetByIdAsync(savingsAccount.UserId);

                var savingsAccountPrincipal = await _savingsAccountService.GetByOwnerIdAsync(user.Id);
                var savingsAccountVm = savingsAccountPrincipal.Find(x => x.IsMain == true && x.UserId == savingsAccount.Id);

                savingsAccountVm.Amount += savingsAccount.Amount;
                SaveSavingsAccountViewModel savingsAccountRequest = _mapper.Map<SaveSavingsAccountViewModel>(savingsAccountVm);
                await _savingsAccountService.UpdateAsync(savingsAccountRequest, savingsAccountRequest.Id);

                await _repository.DeleteAsync(savingsAccount);

                return "Se ha borrado la cuenta. Se ha promovido el dinero a la cuenta de ahorro principal.";
            }

            await _repository.DeleteAsync(savingsAccount);
            return "Se ha borrado la cuenta";
        }*/
    }
}
