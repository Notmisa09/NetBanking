using NetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class Beneficiary : AuditableEntity
    {
        public string AccountId { get; set; }
        public string BeneficiaryId { get; set; }
    }
}
