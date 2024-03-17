using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class SavingsAccountRepository : GenericRepository<SavingsAccount>
    {
        private readonly NetBankingContext _context;
        private readonly DbSet<SavingsAccount> _entities;
        public SavingsAccountRepository(NetBankingContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<SavingsAccount>();
        }
    }
}
