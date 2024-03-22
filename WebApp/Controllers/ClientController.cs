using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Domain.Entities;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.ViewModels.Users;


namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICreditCardService _creditCardService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextaccessor;
        private readonly AuthenticationResponse user;
        public ClientController(IClientService clientService,
                                ICreditCardService creditCardService,
                                IHttpContextAccessor contextaccessor,
                                IUserService userService) 
        { 
            _creditCardService = creditCardService;
            _clientService = clientService;
            _contextaccessor = contextaccessor;
            _userService = userService;
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

        //CREDITCARD
        public async Task<IActionResult> CreditCard()
        {
            return View(await _userService.GetByIdAsync(user.Id));
        }

        [HttpPost]
        public async Task<IActionResult> CreditCard(SaveUserViewModel vm)
        {
            await _creditCardService.CreateCardWithUser(vm);
            return RedirectToRoute(new { controller ="Client" ,  action="Index" });
        }
    }
}
