using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Domain.Entities;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.ViewModels.Users;
using Newtonsoft.Json;
using NetBanking.Core.Domain.Enums;
using NetBanking.Core.Application.ViewModels.Transaction;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly ICreditCardService _creditCardService;
        private readonly IAccountService _accountService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IHttpContextAccessor _contextaccessor;
        private readonly AuthenticationResponse user;
        public ClientController(IClientService clientService,
            ICreditCardService creditCardService,
            IAccountService accountService,
            IBeneficiaryService beneficiaryService,
            IUserService userService,
            IHttpContextAccessor contextAccessor)
        {
            _creditCardService = creditCardService;
            _clientService = clientService;
            _accountService = accountService;
            _beneficiaryService = beneficiaryService;
            _userService = userService;
            _contextaccessor = contextAccessor;
        }
        public async Task<IActionResult> Home()
        {
            var vm = await _clientService.GetAllProductsByClientAsync();
            return View(vm);
        }

        public async Task<IActionResult> Beneficiaries()
        {
            var vm = await _clientService.GetAllBeneficiariesByClientAsync();
            return View(vm);
        }
        public async Task<IActionResult> InitializeTransaction(string TypeOfTransaction)
        {
            var currentUser = HttpContext.Session.Get<AuthenticationResponse>("user");
            RealizeTransaction TransactionRequest = new()
            {
                AllProducts = await _clientService.GetAllProductsByClientAsync(),
                Beneficiaries = await _beneficiaryService.GetByOwnerIdAsync(currentUser.Id)
            };
            return View(TypeOfTransaction, TransactionRequest);
        }

        [HttpPost]
        public async Task<IActionResult> InitializeTransaction(RealizeTransaction svm)
        {
            VerifyTransactionViewModel vm = new()
            {
                Transaction = svm.SaveTransactionViewModel
            };
            string serializedVm = JsonConvert.SerializeObject(vm);
            TempData["confirmTransactionVM"] = serializedVm;
            return RedirectToAction("VerifyTransaction");
        }


        public async Task<IActionResult> VerifyTransaction()
        {
            string serializedVm = TempData["confirmTransactionVM"]?.ToString();
            VerifyTransactionViewModel vm = JsonConvert.DeserializeObject<VerifyTransactionViewModel>(serializedVm);
            // Valido las cosas
            var result = await _clientService.TransactionValidation(vm.Transaction);
            //Si todas las validaciones salen bien, pregunto si quiere hacer la transacción
            if (result.HasError)
            {
                RealizeTransaction transaction = new()
                {
                    AllProducts = await _clientService.GetAllProductsByClientAsync(),
                    SaveTransactionViewModel = vm.Transaction
                };
                transaction.TransactionStatus.HasError = result.HasError;
                transaction.TransactionStatus.Error = result.Error;

                serializedVm = JsonConvert.SerializeObject(transaction);
                TempData["confirmTransactionVM"] = serializedVm;
                switch (vm.Transaction.Type)
                {
                    case TransactionType.ExpressPay:
                        return View(TransactionType.ExpressPay.ToString(), transaction);

                    case TransactionType.LoanPay:
                        return View(TransactionType.LoanPay.ToString(), transaction);

                    case TransactionType.BeneficiaryPay:
                        transaction.Beneficiaries = await _beneficiaryService.GetByOwnerIdAsync(HttpContext.Session.Get<AuthenticationResponse>("user").Id);
                        return View(TransactionType.BeneficiaryPay.ToString(), transaction);

                    case TransactionType.CreditCardPay:
                        return View(TransactionType.CreditCardPay.ToString(), transaction);
                }
            }
            var prod = await _clientService.GetProductByIdAsync(vm.Transaction.ReceiverProductId);
            var titular = await _accountService.GetByIdAsync(prod.UserId);
            SaveTransactionViewModel Transaction = vm.Transaction;
            serializedVm = JsonConvert.SerializeObject(Transaction);
            TempData["executeTransaction"] = serializedVm;
            ViewBag.Name = titular.FirstName + " " + titular.LastName;
            return View("VerifyTransaction");
        }

        public async Task<IActionResult> ExecuteTransaction()
        {
            string serializedVm = TempData["executeTransaction"]?.ToString();
            SaveTransactionViewModel vm = JsonConvert.DeserializeObject<SaveTransactionViewModel>(serializedVm);
            var resultC = await _clientService.RealizeTransaction(vm);
            return RedirectToAction("Home");
        }

        //CREDITCARD
        public async Task<IActionResult> CreditCard()
        {
            return View(await _userService.GetByIdAsync(user.Id));
        }

        [HttpPost]
        public async Task<IActionResult> CreditCard(SaveUserViewModel vm)
        {
            await _creditCardService.CreateCardWithUser(vm);
            return RedirectToRoute(new { controller = "Client", action = "Index" });
        }
    }
}
