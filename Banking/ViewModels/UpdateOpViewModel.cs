using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Collections.Generic;

using BankingLib.Models;

namespace Banking.ViewModels
{
    public enum OperationStatus
    {
        Pending = 0,
        Successful = 1
    }


    public class UpdateOpViewModel : BasicOpViewModel
    {
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public OperationStatus OperationStatus { get; set; }
            = OperationStatus.Pending;

        public override void Validate(ModelStateDictionary modelState)
        {
            base.Validate(modelState);
            if (Amount <= 0)
                modelState.AddModelError("Amount", "Amount must be greater than zero.");

        }

        public override void Clear()
        {
            base.Clear();
            Amount = 0;
            Comment = null;
        }
    }
}
