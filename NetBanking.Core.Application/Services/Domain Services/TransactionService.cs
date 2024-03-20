using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NetBanking.Core.Application.Services
{
    public class TransactionService : GenericService<TransactionViewModel , SaveTransactionViewModel, Transaction>, ITransactionService
    { 
        private readonly IGenericRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public TransactionService(IMapper mapper,
            IGenericRepository<Transaction> repository) : base(mapper, repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
    }
}
