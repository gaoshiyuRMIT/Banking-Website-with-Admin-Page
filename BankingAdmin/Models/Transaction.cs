using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace BankingAdmin.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime ModifyDate { get; set; }
        public int TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }

        [JsonIgnore]
        public virtual Account AccountNumberNavigation { get; set; }
        [JsonIgnore]
        public virtual Account DestAccountNumberNavigation { get; set; }
    }
}
