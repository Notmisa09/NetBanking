using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Enums
{
    public enum TransactionType
    {
        ExpressPay,
        CreditCardPay,
        LoanPay,
        BeneficiaryPay,
        CashAdvance,
        SimpleTransaction
    }
}
