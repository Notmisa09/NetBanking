using NetBanking.Core.Domain.Common;
using NetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        public string EmissorProductId { get; set; }
        public string ReceiverProductId { get; set; }
        public decimal Cantity { get; set; }
        public TransactionType Type { get; set; }

    }
}
