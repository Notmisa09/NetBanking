using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services.Domain_Services
{
    public interface ISavingsAccountService : IGenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>
    {
        //Task SaveUserWIthMainAccount(SaveUserViewModel vm);
        public Task<List<SavingsAccountViewModel>> GetByOwnerIdAsync(string Id);
    }
}
