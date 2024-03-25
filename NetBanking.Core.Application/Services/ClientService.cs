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

        public async Task<TransactionStatusViewModel> RealizeTransaction(SaveTransactionViewModel svm)
        {
            var emissorProduct = await GetProductByIdAsync(svm.EmissorProductId);
            var receiverProduct = await GetProductByIdAsync(svm.ReceiverProductId);

            if (emissorProduct != null && receiverProduct != null)
            {

                #region Determina a donde enviar la actualización del emissorProduct

                int Identificator = Convert.ToInt32(svm.EmissorProductId.Substring(0, 3));

                if (100 <= Identificator && Identificator <= 299) //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(emissorProduct.Id);
                    creditCard.Amount += svm.Amount; //Se le suma a su deuda
                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);

                }

                else if (300 <= Identificator && Identificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(emissorProduct.Id);
                    savingAccount.Amount -= svm.Amount; //Se le resta a su dinero
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(savingAccount), savingAccount.Id);
                }

                else if (600 <= Identificator && Identificator <= 999) //Prestamos comienzan con 3 digitos entre 600 y 999
                {
                    var loan = await _loanService.GetByIdAsync(emissorProduct.Id);
                    loan.Debt += svm.Amount;
                    await _loanService.UpdateAsync(_mapper.Map<SaveLoanViewModel>(loan), loan.Id);
                }
                #endregion


                #region Determina a donde enviar la actualización del receiverProduct

                Identificator = Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3));

                if (100 <= Identificator && Identificator <= 299)  //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(receiverProduct.Id);
                    if (creditCard.Amount - svm.Amount < 0)
                    {
                        var sobra = svm.Amount - creditCard.Amount;
                        svm.Amount = svm.Amount - sobra;

                        SaveTransactionViewModel retorno = new()
                        {
                            Amount = sobra,
                            EmissorProductId = receiverProduct.Id,
                            ReceiverProductId = emissorProduct.Id
                        };
                        await RealizeTransaction(retorno);
                    }
                    creditCard.Amount -= svm.Amount; //Se le resta a su deuda
                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);

                    //Registra la transacción
                    await _transactionService.AddAsync(svm);
                    return new TransactionStatusViewModel()
                    {
                        HasError = false
                    };
                }

                else if (300 <= Identificator && Identificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(receiverProduct.Id);
                    savingAccount.Amount += svm.Amount; //Se le suma a su dinero
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(savingAccount), savingAccount.Id);

                    //Registra la transacción
                    await _transactionService.AddAsync(svm);
                }
                else if (600 <= Identificator && Identificator <= 999) //Prestamos comienzan con 3 digitos entre 600 y 999
                {
                    var loan = await _loanService.GetByIdAsync(svm.ReceiverProductId);

                    if (loan.Debt - svm.Amount < 0)
                    {
                        var sobra = svm.Amount - loan.Debt;
                        svm.Amount = svm.Amount - sobra;

                        SaveTransactionViewModel retorno = new()
                        {
                            Amount = sobra,
                            EmissorProductId = receiverProduct.Id,
                            ReceiverProductId = emissorProduct.Id
                        };
                        await RealizeTransaction(retorno);
                    }
                    loan.Debt -= svm.Amount; //Reduce la deuda
                    await _loanService.UpdateAsync(_mapper.Map<SaveLoanViewModel>(loan), loan.Id);
                }
                #endregion

            }

            return new TransactionStatusViewModel()
            {
                HasError = false
            };

        }

        public async Task<TransactionStatusViewModel> TransactionValidation(SaveTransactionViewModel svm)
        {
            var emissorProduct = await GetProductByIdAsync(svm.EmissorProductId);
            var receiverProduct = await GetProductByIdAsync(svm.ReceiverProductId);
            if (receiverProduct.Id == null)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Cuenta destino inexistente."
                };
            }
            else if (svm.Amount < 1)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Monto inválido."
                };
            }
            else if (emissorProduct.Id != null && receiverProduct.Id != null)
            {
                #region Posibles errores al realizar transacción

                int Identificator = Convert.ToInt32(svm.EmissorProductId.Substring(0, 3));

                if (100 <= Identificator && Identificator <= 299) //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(emissorProduct.Id);

                    if (creditCard.Amount + svm.Amount > creditCard.Limit)
                    {
                        return new TransactionStatusViewModel()
                        {
                            HasError = true,
                            Error = "Usted está sobregirando la tarjeta."
                        };
                    }
                }

                else if (300 <= Identificator && Identificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(emissorProduct.Id);

                    if (savingAccount.Amount - svm.Amount < 0)
                    {
                        return new TransactionStatusViewModel()
                        {
                            HasError = true,
                            Error = "No tiene dinero suficiente"
                        };
                    }
                }
                #endregion
            }
            else if (emissorProduct.Id == receiverProduct.Id)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Transferencia inválida."
                };
            }
            return new TransactionStatusViewModel()
            {
                HasError = false
            };
        }

        public async Task<bool> ProductExists(string Id)
        {
            var savingsAccount = await _savingsAccountService.FindAllAsync(x => x.Id == Id);
            var creditCard = await _creditCardService.FindAllAsync(x => x.Id == Id);
            var loan = await _loanService.FindAllAsync(x => x.Id == Id);
            if (savingsAccount.Count != 0 || creditCard.Count != 0 || loan.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<BaseProduct> GetProductByIdAsync(string Id)
        {
            BaseProduct product = new BaseProduct();
            if (Id.Length > 3)
            {
                //Asumiendo que las tarjetas de credito comienzan con 3 digitos entre 100 y 299
                if (100 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 299)
                {
                    var entity = await _creditCardService.GetByIdAsync(Id);
                    if (entity != null)
                    {
                        product.Amount = entity.Amount;
                        product.Id = entity.Id;
                        product.UserId = entity.UserId;
                        product.CreatedById = entity.CreatedById;
                        product.CreatedDate = entity.CreatedDate;
                    }

                }
                //Asumiendo que las cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                else if (300 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 599)
                {
                    var entity = await _savingsAccountService.GetByIdAsync(Id);
                    if (entity != null)
                    {
                        product.Amount = entity.Amount;
                        product.Id = entity.Id;
                        product.UserId = entity.UserId;
                        product.CreatedById = entity.CreatedById;
                        product.CreatedDate = entity.CreatedDate;
                    }
                }
                //Asumiendo que los préstamos comienzan con 3 digitos entre 600 y 999
                else if (600 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 999)
                {
                    var entity = await _loanService.GetByIdAsync(Id);
                    if (entity != null)
                    {
                        product.Amount = entity.Amount;
                        product.Id = entity.Id;
                        product.UserId = entity.UserId;
                        product.CreatedById = entity.CreatedById;
                        product.CreatedDate = entity.CreatedDate;
                    }
                }
            }
            else
            {
                product = null;
            }
            return product;
        }

    }
}