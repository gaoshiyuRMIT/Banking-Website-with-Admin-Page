using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    public enum AccountType : int
    {
        Savings = 0,
        Checking = 1
    }

    public partial class Account : AModifyDate
    {
        [Display(Name = "Minimum Balance")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Saving Account Must Be Zero and more")]
        public decimal MinBalance {
            get => AccountType == AccountType.Savings ? 0 : 200;
        }
        [Display(Name = "Minimum Opening Balance")]
        [Range(200, (double)decimal.MaxValue, ErrorMessage = "Saving Account Opening Balance Must Be More Than $200")]
        public decimal MinOpeningBalance
        {
            get => AccountType == AccountType.Savings ? 100 : 500;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        [Range(0, 9999, ErrorMessage = "No More Than 4 digits")]
        public int AccountNumber { get; set; }


        [Display(Name = "Account Type")]
        [Required]
        public AccountType AccountType { get; set; }


        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }


        [Display(Name = "Balance")]
        [Required, DataType(DataType.Currency),Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Balance { get; set; }


        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<BillPay> BillPays { get; set; }
    }

    public partial class Account : AModifyDate
    {
        public void Deposit(decimal amount, string comment)
        {
            Balance += amount;
            ModifyDate = DateTime.UtcNow;
            Transaction t = new Transaction
            {
                TransactionType = TransactionType.Deposit,
                Amount = amount,
                ModifyDate = DateTime.UtcNow,
                Comment = comment
            };
            Transactions.Add(t);
        }

        public void Withdraw(decimal amount, string comment)
        {
            Balance -= amount;
            ModifyDate = DateTime.UtcNow;
            Transaction t = new Transaction
            {
                TransactionType = TransactionType.Withdrawal,
                Amount = amount,
                ModifyDate = DateTime.UtcNow,
                Comment = comment
            };
            Transactions.Add(t);
            // deals with service fee
            if (t.ShouldCharge)
            {
                int nShouldCharge = Transactions.Where(x => x.ShouldCharge).Count();
                if (nShouldCharge > Transaction.NFreeTransaction)
                    Transactions.Add(t.CreateServiceTransaction());
            }
        }

        public void Transfer(Account destAccount, decimal amount, string comment)
        {
            Balance -= amount;
            ModifyDate = DateTime.UtcNow;
            destAccount.Balance += amount;
            destAccount.ModifyDate = DateTime.UtcNow;
            Transaction t = new Transaction
            {
                TransactionType = TransactionType.Transfer,
                Amount = amount,
                DestAccount = destAccount,
                ModifyDate = DateTime.UtcNow,
                Comment = comment
            };
            Transactions.Add(t);
            if (t.ShouldCharge)
            {
                int nShouldCharge = Transactions.Where(x => x.ShouldCharge).Count();
                if (nShouldCharge > Transaction.NFreeTransaction)
                    Transactions.Add(t.CreateServiceTransaction());
            }

        }
    }
}
