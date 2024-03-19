using NetBanking.Core.Application.ViewModels.Transaction;
using System.Transactions;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface ITransactionService : IGenericService<TransactionViewModel , SaveTransactionViewModel , Transaction>
    {

    }
}
