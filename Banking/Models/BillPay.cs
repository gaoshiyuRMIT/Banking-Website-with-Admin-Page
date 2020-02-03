using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Banking.Extensions;

namespace Banking.Models
{
    public enum BillPayPeriod
    {
        OnceOff = 0,
        Monthly = 1,
        Quarterly = 2,
        Annually = 3
    };

    public class BillPay : AModifyDate
    {


        [Display(Name = "Bill Pay ID")]
        [Range(0, 9999, ErrorMessage = "No More Than 4 digits")]
        public int BillPayID { get; set; }

        [ForeignKey("Account")]
        [Display(Name = "Account Number")]
        [Required, Range(0, 9999, ErrorMessage = "No More Than 4 digits")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }


        [Display(Name = "Payee ID")]
        public int PayeeID { get; set; }
        public virtual Payee Payee { get; set; }

        [Display(Name = "Amount")]
        [Required, DataType(DataType.Currency), Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }


        [Display(Name = "Schedule Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ScheduleDate { get; set; }

        [Display(Name = "Period")]
        [Required, StringLength(1, MinimumLength = 1, ErrorMessage = "No Such Period")]
        public BillPayPeriod Period { get; set; }

        [Display(Name = "Comment")]
        [StringLength(255, ErrorMessage = "No More Than 255 characters")]
        public string Comment { get; set; }

        [Display(Name = "Schedule Date Local")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ScheduleDateLocal => ScheduleDate.ToLocalTime();
        
        public DateTime? NextDateTime
        {
            get
            {
                if (ScheduleDate > DateTime.UtcNow)
                    return ScheduleDate;
                if (Period == BillPayPeriod.OnceOff)
                    return null;
                int nMonth = Period.ToMonth().Value;
                DateTime _scheduleDate = ScheduleDate;
                while (_scheduleDate <= DateTime.UtcNow)
                {
                    _scheduleDate.AddMonths(nMonth);
                }
                return _scheduleDate;
            }
        }

        public bool ExecuteBillPay(out string errMsg)
        {
            errMsg = string.Empty;
            if (Account.Balance - Amount < Account.MinBalance)
            {
                errMsg = "The amount after deduction would be lower than the minimum allowed.";
                if (Period != BillPayPeriod.OnceOff)
                    ScheduleDate = NextDateTime.Value;
                return false;
            }
            Account.Balance -= Amount;
            Account.Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.BillPay,
                Amount = Amount,
                ModifyDate = DateTime.UtcNow
            });
            // update schedule date
            if (Period != BillPayPeriod.OnceOff)
                ScheduleDate = NextDateTime.Value;
            return true;
        }
    }

}
