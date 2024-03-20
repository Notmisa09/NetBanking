using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Client
{
    public class GetAllProductsByClientViewModel
    {
        public List<CreditCardViewModel> CreditCards { get; set;}
        public List<LoanViewModel> Loans { get; set;}
        public List<SavingsAccountViewModel> SavingsAccounts { get; set;}
    }
}
