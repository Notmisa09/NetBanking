namespace NetBanking.Core.Application.ViewModels.SavingsAccount
{
    public class SaveSavingsAccountViewModel
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsMain { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
