using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Transaction;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICreditCardService _creditCardService;
        public ClientController(IClientService clientService, 
            ICreditCardService creditCardService) 
        {
            _creditCardService = creditCardService;
            _clientService = clientService;
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
        public async Task<IActionResult> ExpressPay()
        {
            ExpressPayViewModel expressPay = new()
            {
                AllProducts = await _clientService.GetAllProductsByClientAsync()
            };
            return View(expressPay);
        }

        /*[HttpPost]
        public async Task<IActionResult> ExpressPay(ExpressPayViewModel svm)
        {
            
        }*/

        /*public async Task<IActionResult> CreditCardRequest(SaveUserViewModel vm)
        {
            await _creditCardService.CreateCardWithUser(vm);
            return RedirectToRoute(new {controller="Client", action="Index"});
        }*/
    }
}
