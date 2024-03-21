using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class LoanService : GenericService<SaveLoanViewModel, LoanViewModel, Loan>, ILoanService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILoanRepository _repository;
        public LoanService(
            IMapper mapper,
            ILoanRepository repository) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<LoanViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<LoanViewModel>>(list);
        }

        public async Task<string> Delete(string Id)
        {
            var loan = await _repository.GeEntityByIDAsync(Id);

            if (loan.Debt >= 0)
            {
                return $"Este usuario tiene una deuda pendiente de {loan.Debt}.";
            }
            else
            {
                await _repository.DeleteAsync(loan);
                return "Se ha borrado el prestamo";
            }
        }
    }
}
