using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.ViewModels.Client;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Loan;

namespace NetBanking.Core.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ITransactionService _transactionService;

        public ClientService(
            IAccountService accountService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISavingsAccountService savingsAccountService,
            ICreditCardService creditCardService,
            ILoanService loanService,
            IBeneficiaryService beneficiaryService,
            ITransactionService transactionService
            )
        {
            _accountService = accountService;
            _beneficiaryService = beneficiaryService;
            _creditCardService = creditCardService;
            _transactionService = transactionService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loanService = loanService;
            _savingsAccountService = savingsAccountService;
        }

        public async Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            GetAllProductsByClientViewModel vm = new()
            {
                SavingsAccounts = await _savingsAccountService.GetByOwnerIdAsync(userId),
                CreditCards = await _creditCardService.GetByOwnerIdAsync(userId),
                Loans = await _loanService.GetByOwnerIdAsync(userId)
            };

            return vm;
        }

        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiariesByClientAsync()
        {
            var userId = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id;
            return await _beneficiaryService.GetByOwnerIdAsync(userId);
        }

        public async Task RealizeTransaction(SaveTransactionViewModel svm)
        {
            var emissorProduct = await GetProductByIdAsync(svm.EmissorProductId);
            var receiverProduct = await GetProductByIdAsync(svm.ReceiverProductId);
            if (emissorProduct != null && receiverProduct != null)
            {
                emissorProduct.Amount -= svm.Amount;
                receiverProduct.Amount += svm.Amount;

                #region Determina a donde enviar la actualización del emissorProduct
                //Asumiendo que las targetas de credito comienzan con 3 digitos entre 100 y 299
                if (100 >= Convert.ToInt32(svm.EmissorProductId.Substring(0, 3)) && Convert.ToInt32(svm.EmissorProductId.Substring(0, 3)) <= 299)
                {
                    var creditCard = await _creditCardService.GetByIdAsync(emissorProduct.Id);
                    creditCard.Debt += svm.Amount;
                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);
                }
                //Asumiendo que las cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                else if (300 >= Convert.ToInt32(svm.EmissorProductId.Substring(0, 3)) && Convert.ToInt32(svm.EmissorProductId.Substring(0, 3)) <= 599)
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(emissorProduct.Id);
                    savingAccount.Amount = emissorProduct.Amount;
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(emissorProduct), emissorProduct.Id);
                }
                #endregion

                #region Determina a donde enviar la actualización del receiverProduct
                //Asumiendo que las targetas de credito comienzan con 3 digitos entre 100 y 299
                if (100 >= Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3)) && Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3)) <= 299)
                {
                    var creditCard = await _creditCardService.GetByIdAsync(receiverProduct.Id);
                    creditCard.Debt -= svm.Amount;
                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);
                }
                //Asumiendo que las cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                else if (300 >= Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3)) && Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3)) <= 599)
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(receiverProduct.Id);
                    savingAccount.Amount = emissorProduct.Amount;
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(savingAccount), savingAccount.Id);
                }
                #endregion

            }
            await _transactionService.AddAsync(svm);
        }

        /*public async Task<bool> VerifyProductExistence(string Id)
        {
            var savingsAccount = await _savingsAccountService.FindAllAsync(x => x.Id == Id);
            var creditAccount = await _creditCardService.FindAllAsync(x => x.Id == Id);
            var loan
            if ()
        }*/

        public async Task<BaseProduct> GetProductByIdAsync(string Id)
        {
            BaseProduct product = null;
            //Asumiendo que las targetas de credito comienzan con 3 digitos entre 100 y 299
            if (100 >= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) < 300)
            {
                var entity = await _creditCardService.GetByIdAsync(Id);
                product.Amount = entity.Amount;
                product.Id = entity.Id;
            }
            //Asumiendo que las cuentas de ahorro comienzan con 3 digitos entre 300 y 599
            else if (300 >= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 599)
            {
                var entity = await _savingsAccountService.GetByIdAsync(Id);
                product.Amount = entity.Amount;
                product.Id = entity.Id;
            }
            //Asumiendo que los préstamos comienzan con 3 digitos entre 600 y 999
            else if (300 >= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 599)
            {
                var entity = await _loanService.GetByIdAsync(Id);
                product.Amount = entity.Amount;
                product.Id = entity.Id;
            }
            return product;
        }

    }
}