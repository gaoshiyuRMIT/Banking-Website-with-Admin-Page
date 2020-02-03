using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Banking.Models;
using Banking.Extensions;

namespace Banking.ViewModels
{
    public class WithdrawViewModel : UpdateOpViewModel
    {
        public override void Validate(ModelStateDictionary modelState)
        {
            base.Validate(modelState);

            if (Account.Balance - Amount < Account.MinBalance)
                modelState.AddModelError("Amount",
                    "balance would be lower than the allowed minimum after deduction.");
        }
    }
}
