using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ICreditCardRepository _repository;
        public CreditCardService(ICreditCardRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<List<CreditCardViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<CreditCardViewModel>>(list);
        }
        public async Task<string> Delete(string Id)
        {
            var creditCard = await _repository.GeEntityByIDAsync(Id);

            if (creditCard.Debt >= 0)
            {
                return $"Este usuario tiene una deuda pendiente de {creditCard.Debt}.";
            }
            else
            {
                await _repository.DeleteAsync(creditCard);
                return "Se ha borrado la tarjeta de credito";
            }
        }
    }
}
