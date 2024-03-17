using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.IRepositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Transaction> _entities;
        public TransactionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Transaction>();
        }
    }
}
