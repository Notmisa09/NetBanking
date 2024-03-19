using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Transaction;
using System.Transactions;
namespace NetBanking.Core.Application.Services
{
    public class TransactionService : GenericService<TransactionViewModel, SaveTransactionViewModel, Transaction>,
        ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _repository;
        public TransactionService(Mapper mapper, ITransactionRepository
            repository) : base(mapper ,repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}
