using System.ComponentModel.DataAnnotations;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public string? Id { get; set; }
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Hace falta la ceunta beneficiaria")]
        [DataType(DataType.Text)]
        public string BeneficiaryId { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
