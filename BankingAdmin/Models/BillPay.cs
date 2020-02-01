using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace BankingAdmin.Models
{
    public partial class BillPay
    {
        public int BillPayId { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AccountNumber { get; set; }
        public int PayeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int Period { get; set; }
        public string Comment { get; set; }

        [JsonIgnore]
        public virtual Account AccountNumberNavigation { get; set; }
        public virtual Payee Payee { get; set; }
    }
}
