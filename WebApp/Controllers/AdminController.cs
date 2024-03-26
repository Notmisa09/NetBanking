﻿using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.Singelton;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.Users;

namespace WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        private readonly ISavingsAccountService _savingAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IClientService _clientService;

        public AdminController( IAdminService adminService,
                                ILoanService loanService,
                                IUserService userService,
                                ISavingsAccountService savingAccountService,
                                ICreditCardService creditCardService,
                                IClientService clientService)
        {
            _clientService = clientService;
            _adminService = adminService;
            _userService = userService;
            _creditCardService = creditCardService;
            _savingAccountService = savingAccountService;
            _loanService = loanService; 
        }

        //INDEX
        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(StringStorage.Instance.GetStoredString()))
            {
                ViewBag.Error = StringStorage.Instance.GetStoredString();
                StringStorage.Instance.SetStoredString("");
            }

            return View(await _adminService.GetAllAsync());
        }

        //LOAN
        public IActionResult ApproveLoan(string Id)
        {
            TempData["Id"] = Id;
            return View(new SaveLoanViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SaveLoan(string userId, decimal monto)
        {
            SaveLoanViewModel vm = new();
            vm.UserId = userId;
            vm.Limit = monto;
            await _loanService.AddAsync(vm);
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }


        //CREDITCARD
        [HttpPost]
        public async Task<IActionResult> SaveCreditCard(string userId, decimal monto)
        {
            SaveCreditCardViewModel vm = new();
            vm.UserId = userId;
            vm.Limit = monto;
            await _creditCardService.AddAsync(vm);
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }

        //ACCOUTNT
        [HttpPost]
        public async Task<IActionResult> ProductAddSavingAccount(string userId, decimal monto)
        {
            var user = await _userService.GetByIdAsync(userId);
            user.InitialAmount = monto;
            await _savingAccountService.SaveUserWIthAccount(user);
            return RedirectToRoute(new { controller = "ProductAdd", action = "Index" });
        }

        //DASHBORAD
        public async Task<IActionResult> DashBoard()
        {
            return View(await _adminService.GetDashboard());
        }

        //REGISTER USERS FROM ADMIN
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        public async Task<IActionResult> ViewProducts(string Id)
        {
            return View(await _clientService.GetAllProductsByClientAsync(Id));
        }


        public async Task<IActionResult> deleteCreditCard(string Id)
        {
            await _creditCardService.Delete(Id);
            return RedirectToRoute(new { controller = "Admin", action = "ViewProducts" });
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            var origin = Request.Headers["origin"];
            ServiceResult response = await _userService.RegisterAsync(vm, origin, vm.Role);
            if (!response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        //LOGOUT
        public async Task<IActionResult> LogOut()
        {
            await _userService.SingOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //CHANGE USER STATUS
        public async Task<IActionResult> ChangeStatus(string Id)
        {
            var error = await _adminService.ChangeAccountStatus(Id);
            StringStorage.Instance.SetStoredString(error);
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }


        //EDIT USER
        public async Task<IActionResult> Edit(string Id)
        {
            return View("Register", await _userService.GetByIdAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", vm);
            };
            ServiceResult response = await _userService.UpdateAsync(vm);
            if (response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View("Register", vm);
            }
            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }

        //PRODUCT ADD
        public async Task<IActionResult> ProductAdd(string Id)
        {
            return View(await _userService.GetByIdAsync(Id));
        }
    }
}
