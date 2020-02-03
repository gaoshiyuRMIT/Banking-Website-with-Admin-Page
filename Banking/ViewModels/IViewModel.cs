using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Banking.ViewModels
{
    public interface IViewModel
    {
        public void Validate(ModelStateDictionary modelState);
        

        public void Clear();
        

    }
}
