namespace NetBanking.Core.Application.Dtos.Account
{
    public class DtoAccounts
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
        public string ImageURL { get; set; }
    }
}
