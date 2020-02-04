using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingLib.Models
{
    public enum TransactionType : int
    {
        Deposit = 0,
        Withdrawal = 1,
        Transfer = 2,
        ServiceCharge = 3,
        BillPay = 4
    }

    public class Transaction : AModifyDate
    {
        public const int NFreeTransaction = 4;
        public static readonly Dictionary<TransactionType, decimal> ServiceFee =
            new Dictionary<TransactionType, decimal>
            {
                [TransactionType.Withdrawal] = 0.1M,
                [TransactionType.Transfer] = 0.2M
            };
        public bool ShouldCharge {
            get => TransactionType == TransactionType.Transfer
                || TransactionType == TransactionType.Withdrawal;
        }
        [Display(Name = "Transaction ID")]
        [Range (0,9999, ErrorMessage ="No Zero And No More Than 4 Digits")]
        public int TransactionID { get; set; }

        [Display(Name = "Transaction Type")]
        [Required]
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [Display(Name = "Destination Account Number")]
        [Range(0, 9999, ErrorMessage = "No More Than 4 digits")]
        public int? DestAccountNumber { get; set; }
        public virtual Account DestAccount { get; set; }

        [Display(Name = "Amount")]
        [Required, DataType(DataType.Currency), Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Display(Name = "Comment")]
        [StringLength(255, ErrorMessage = "No More Than 255 characters")]
        public string Comment { get; set; }

        public Transaction CreateServiceTransaction()
        {
            if (!ShouldCharge)
                return null;
            return new Transaction
            {
                TransactionType = TransactionType.ServiceCharge,
                Amount = ServiceFee[TransactionType],
                Comment = Comment + " service charge",
                DestAccount = DestAccount,
                ModifyDate = DateTime.UtcNow
            };
        }
    }
}
