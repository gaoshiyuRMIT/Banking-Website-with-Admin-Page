﻿using System;
using System.Collections.Generic;

namespace BankingAdmin.Models
{
    public partial class Account
    {
        public Account()
        {
            BillPay = new HashSet<BillPay>();
            TransactionAccountNumberNavigation = new HashSet<Transaction>();
            TransactionDestAccountNumberNavigation = new HashSet<Transaction>();
        }

        public int AccountNumber { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AccountType { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<BillPay> BillPay { get; set; }
        public virtual ICollection<Transaction> TransactionAccountNumberNavigation { get; set; }
        public virtual ICollection<Transaction> TransactionDestAccountNumberNavigation { get; set; }
    }
}
