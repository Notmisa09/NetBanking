using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Client;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IClientService
    {
        Task<List<BeneficiaryViewModel>> GetAllBeneficiariesByClientAsync();
        Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync();
        Task<BaseProduct> GetProductByIdAsync(string Id);
        Task RealizeTransaction(SaveTransactionViewModel svm);
    }
}