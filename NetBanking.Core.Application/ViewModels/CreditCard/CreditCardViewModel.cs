namespace NetBanking.Core.Application.ViewModels.CreditCard
{
    public class CreditCardViewModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Debt { get; set; }
    }
}
