﻿using NetBanking.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Domain.Entities
{
    public class BaseProduct : AuditableEntity
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
