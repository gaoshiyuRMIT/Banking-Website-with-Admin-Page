using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using BankingLib.Models;
using BankingLib.Extensions;

namespace Banking.ViewModels
{
    public enum BillPayEditOp
    {
        Create = 0,
        Edit = 1
    }

    public enum PayeeOp
    {
        Choose = 0,
        Create = 1
    }

    public class BillPayEditViewModel : UpdateOpViewModel
    {
        private DateTime _scheduleDate;

        public BillPayPeriod Period { get; set; }
        public List<SelectListItem> PeriodSelect
        {
            get
            {
                List<SelectListItem> select = new List<SelectListItem>();
                foreach (var name in Enum.GetNames(typeof(BillPayPeriod)))
                {
                    object bp = Enum.Parse(typeof(BillPayPeriod), name);
                    select.Add(new SelectListItem
                    {
                        Text = ((BillPayPeriod)bp).ToFriendlyString(),
                        Value = name
                    });
                }
                return select;
            }
        }
        public DateTime ScheduleDateLocal
        {
            get => _scheduleDate.ToLocalTime();
            set => _scheduleDate = value.SpecifySecond(0).ToUniversalTime();
            
        }
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate
        {
            get => _scheduleDate;
            set => _scheduleDate = DateTime
                .SpecifyKind(value.SpecifySecond(0), DateTimeKind.Utc);
        }
        public Payee Payee { get; set; }

        public BillPayEditOp BillPayEditOp { get; set; }

        public override void Validate(ModelStateDictionary modelState)
        {
            base.Validate(modelState);

            if (ScheduleDate <= DateTime.UtcNow)
                modelState.AddModelError("ScheduleDateLocal",
                    "Schedule date must be in the future.");
            if (Payee == null)
                modelState.AddModelError("Payee",
                    "Must specify payee.");
        }

        public BillPay GenerateBillPay() 
        {
            return new BillPay 
            {
                Period = Period,
                ScheduleDate = ScheduleDate,
                Comment = Comment,
                Amount = Amount,
                Account = Account,
                Payee = Payee
            };

        }

        public static BillPayEditViewModel FromBillPay(BillPay billPay) 
        {
            return new BillPayEditViewModel {
                AccountType = billPay.Account.AccountType,
                ScheduleDate = billPay.ScheduleDate,
                Period = billPay.Period,
                BillPayEditOp = BillPayEditOp.Edit,
                Amount = billPay.Amount,
                Payee = billPay.Payee
            };
        }
    }
}
