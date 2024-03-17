using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public string AccountId { get; set; }
        public SaveUserViewModel AccountUser { get; set; }
        public string BeneficiaryId { get; set; }
        public SaveUserViewModel BeneficiaryUser { get; set; }
    }
}
