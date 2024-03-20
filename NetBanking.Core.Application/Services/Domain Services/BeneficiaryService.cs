using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IBeneficiaryRepository _repository;
        public BeneficiaryService(
            IMapper mapper,
            IBeneficiaryRepository repository) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<BeneficiaryViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.AccountId == Id);
            return _mapper.Map<List<BeneficiaryViewModel>>(list);
        }
    }
}