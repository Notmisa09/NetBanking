using NetBanking.Core.Domain.Common;

namespace NetBanking.Core.Domain.Entities
{
    public class Beneficiary : AuditableEntity
    {
        public string AccountId { get; set; }
        public string BeneficiaryId { get; set; } 
    }
}
