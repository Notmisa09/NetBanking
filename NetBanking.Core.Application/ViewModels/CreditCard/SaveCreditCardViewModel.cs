namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class SaveCreditCardViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public decimal Limit { get; set; }
        public decimal Debt { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
