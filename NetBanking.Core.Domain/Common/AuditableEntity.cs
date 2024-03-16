using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Common
{
    public class AuditableEntity
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedById { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedById { get; set; }
    }
}
