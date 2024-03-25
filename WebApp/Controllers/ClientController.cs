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
using NetBanking.Core.Application.ViewModels.Beneficiary;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            user = _contextaccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
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

        public async Task<IActionResult> AddBeneficiary()
        {
            var svm = new SaveBeneficiaryViewModel();
            svm.UserId = user.Id;
            return View(svm);
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiary(SaveBeneficiaryViewModel svm)
        {
            if (!ModelState.IsValid && ModelState["BeneficiaryId"].ValidationState == ModelValidationState.Valid)
            {
                return View(svm);
            }
            var result = await _clientService.AddBeneficiary(svm);
            if (result.HasError)
            {
                return View(result);
            }
            else
            {
                return View("Beneficiaries");
            }
        }

        public async Task<IActionResult> DeleteBeneficiary(string Id)
        {
            await _clientService.DeleteBeneficiary(Id);
            return View("Beneficiaries");
        }

        public async Task<IActionResult> InitializeTransaction(string TypeOfTransaction)
        {
            RealizeTransaction TransactionRequest = new()
            {
                AllProducts = await _clientService.GetAllProductsByClientAsync(),
                Beneficiaries = await _beneficiaryService.GetByOwnerIdAsync(user.Id),
                SaveTransactionViewModel = new SaveTransactionViewModel()
            };
            TransactionRequest.SaveTransactionViewModel.Type = (TransactionType) Enum.Parse(typeof(TransactionType), TypeOfTransaction);
            return View(TypeOfTransaction, TransactionRequest);
        }

        [HttpPost]
        public async Task<IActionResult> InitializeTransaction(RealizeTransaction svm)
        {
            VerifyTransactionViewModel vm = new()
            {
                Transaction = svm.SaveTransactionViewModel
            };
            vm.Transaction.Type = svm.SaveTransactionViewModel.Type;
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
                transaction.SaveTransactionViewModel.Type = vm.Transaction.Type;
                transaction.TransactionStatus.HasError = result.HasError;
                transaction.TransactionStatus.Error = result.Error;

                serializedVm = JsonConvert.SerializeObject(transaction);
                TempData["confirmTransactionVM"] = serializedVm;
                return View(vm.Transaction.Type.ToString(), transaction);
            }
            var prod = await _clientService.GetProductByIdAsync(vm.Transaction.ReceiverProductId);
            var titular = await _accountService.GetByIdAsync(prod.UserId);
            SaveTransactionViewModel Transaction = vm.Transaction;

            serializedVm = JsonConvert.SerializeObject(Transaction);
            TempData["executeTransaction"] = serializedVm;
            if(vm.Transaction.Type == TransactionType.ExpressPay || vm.Transaction.Type == TransactionType.BeneficiaryPay)
            {
                ViewBag.Name = titular.FirstName + " " + titular.LastName;
                return View("VerifyTransaction");
            }
            else
            {
                return RedirectToAction("ExecuteTransaction");
            }
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

        //DELETE BENEFICIARY
        public async Task<IActionResult> RemoveBeneficiary(string Id)
        {
            return View(await _beneficiaryService.GetByIdAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveeBeneficiaryTrue(string Id)
        {
            await _beneficiaryService.(vm);
            return RedirectToRoute(new { controller = "Client", action = "Index" });
        }


    }
}
