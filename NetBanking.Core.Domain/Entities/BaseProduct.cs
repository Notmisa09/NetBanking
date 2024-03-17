using NetBanking.Core.Domain.Common;

namespace NetBanking.Core.Domain.Entities
{
    public abstract class BaseProduct : AuditableEntity
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
