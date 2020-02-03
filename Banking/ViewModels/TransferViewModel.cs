using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using Microsoft.AspNetCore.Mvc.ModelBinding;

using Banking.Models;

namespace Banking.ViewModels
{
    public class TransferViewModel : UpdateOpViewModel
    {
        [Range(0, 9999,
            ErrorMessage="account number is 4 digits long")]
        [Display(Name = "Destination Account Number")]
        public int DestAccountNumber { get; set; }
        public Account DestAccount { get; set; }

        public override void Validate(ModelStateDictionary modelState)
        {
            base.Validate(modelState);
            if (Account.Balance - Amount < Account.MinBalance)
                modelState.AddModelError("Amount",
                    "balance would be lower than the allowed minimum after deduction.");
            if (Account.AccountNumber == DestAccountNumber)
                modelState.AddModelError("DestAccountNumber",
                    "Cannot transfer from and to the same account.");
            if (DestAccount == null)
                modelState.AddModelError("DestAccountNumber",
                    "The provided destination account number is invalid.");
        }

        public override void Clear()
        {
            base.Clear();
            DestAccountNumber = 0;
        }
    }
}
