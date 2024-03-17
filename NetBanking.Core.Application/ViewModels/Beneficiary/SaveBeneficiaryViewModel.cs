using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Hace falta la ceunta beneficiaria")]
        [DataType(DataType.Text)]
        public string BeneficiaryId { get; set; }
    }
}
