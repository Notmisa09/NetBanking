namespace NetBanking.Core.Application.ViewModels.Loan
{
    public class SaveLoanViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Debt { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
