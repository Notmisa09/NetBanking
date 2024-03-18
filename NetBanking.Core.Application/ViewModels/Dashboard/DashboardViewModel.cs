using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int AllTransaction { get; set; }
        public int AllPaymentsNumber { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
        public int AllPayments { get; set; }
        public int AssignedProduct { get; set; }
    }
}
