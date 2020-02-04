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

    public enum BillPayStatus
    {
        Pending = 0,
        Succeeded = 1,
        Failed = 2,
        Blocked = 3
    }

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ScheduleDateLocal => ScheduleDate.ToLocalTime();
        public BillPayStatus Status {get;set;}
        
        private DateTime _statusModifyDate = DateTime.UtcNow;
        public DateTime StatusModifyDate 
        {
            get => _statusModifyDate; 
            set => _statusModifyDate = value;
        }
        
        public DateTime StatusModifyDateLocal => StatusModifyDate.ToLocalTime();

        [Display(Name = "Status")]
        public string StatusDesc 
        {
            get 
            {
                if (Status == BillPayStatus.Pending)
                    return "Pending";
                return $"Execution {Status} at {StatusModifyDateLocal:dd/MM/yyyy HH:mm}";
            }
        }
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
                    _scheduleDate = _scheduleDate.AddMonths(nMonth);
                }
                return _scheduleDate;
            }
        }

        private void UpdateScheduleDate()
        {
            if (Period != BillPayPeriod.OnceOff)
                ScheduleDate = NextDateTime.Value;
        }

        public void Succeed()
        {
            Status = BillPayStatus.Succeeded;
            StatusModifyDate = DateTime.UtcNow;
            UpdateScheduleDate();
        }

        public void Fail()
        {
            Status = BillPayStatus.Failed;
            StatusModifyDate = DateTime.UtcNow;
            UpdateScheduleDate();
        }

        public void Block()
        {
            Status = BillPayStatus.Blocked;
            StatusModifyDate = DateTime.UtcNow;
        }

        public void Unblock()
        {
            Status = BillPayStatus.Pending;
            StatusModifyDate = DateTime.UtcNow;
            UpdateScheduleDate();
        }

        public bool ExecuteBillPay(out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                if (Account.Balance - Amount < Account.MinBalance)
                {
                    throw new Exception("The amount after deduction would be lower than the minimum allowed.");
                }
                Account.Balance -= Amount;
                Account.Transactions.Add(new Transaction
                {
                    TransactionType = TransactionType.BillPay,
                    Amount = Amount,
                    ModifyDate = DateTime.UtcNow
                });
                Succeed();
                return true;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                Fail();
                return false;
            }
        }
    }

}
