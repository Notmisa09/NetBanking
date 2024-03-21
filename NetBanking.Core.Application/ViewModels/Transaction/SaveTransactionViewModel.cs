﻿using NetBanking.Core.Domain.Enums;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class SaveTransactionViewModel
    {
        public string? Id { get; set; }
        public string EmissorProductId { get; set; }
        public string ReceiverProductId { get; set; }
        public decimal Cantity { get; set; }
        public TransactionType Type { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
