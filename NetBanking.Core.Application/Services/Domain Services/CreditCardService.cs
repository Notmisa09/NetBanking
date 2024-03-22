using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardRepository _repository;
        private readonly AuthenticationResponse user;
        public CreditCardService(ICreditCardRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CreditCardViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<CreditCardViewModel>>(list);
        }

        public async Task CreateCardWithUser(SaveUserViewModel vm)
        {
            var code = string.Empty;
            string generatedCode = string.Empty;
            CreditCardViewModel existingCreditCard;

            for (int i = 0; i <= 2; i++)
            {
                do
                {
                    generatedCode = CodeGeneratorHelper.GenerateCode(code, typeof(CreditCard));
                    existingCreditCard = await GetByIdAsync(generatedCode);

                    if (existingCreditCard == null)
                    {
                        break;
                    }

                } while (existingCreditCard != null);
            }

            CreditCard card = new CreditCard()
            {
                Amount = 150000m,
                UserId = vm.Id,
                CreatedById = "Default",
                Debt = 0,
                CreatedDate = DateTime.UtcNow,
                Id = generatedCode
            };
            await _repository.AddAsync(card);
        }


        public override async Task<string> Delete(string Id)
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
