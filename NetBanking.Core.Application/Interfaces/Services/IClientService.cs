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
        Task<TransactionStatusViewModel> RealizeTransaction(SaveTransactionViewModel svm);
        Task<bool> ProductExists(string Id);
        Task<TransactionStatusViewModel> TransactionValidation(SaveTransactionViewModel svm);
    }
}