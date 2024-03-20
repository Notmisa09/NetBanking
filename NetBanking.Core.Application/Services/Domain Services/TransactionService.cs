using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;
using System.Linq.Expressions;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class TransactionService : GenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>, ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Task<List<TransactionViewModel>> FindAllAsync(Expression<Func<Domain.Entities.Transaction, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TransactionViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.EmissorProductId == Id);
            return _mapper.Map<List<TransactionViewModel>>(list);
        }
    }
}
