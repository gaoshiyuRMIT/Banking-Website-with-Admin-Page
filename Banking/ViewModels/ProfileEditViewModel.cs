using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using BankingLib.Models;

namespace Banking.ViewModels
{
    public class ProfileEditViewModel : Customer
    {
        [StringLength(20)]
        public string Password { get; set; }
        public OperationStatus OperationStatus {get;set;} = OperationStatus.Pending;

        public static ProfileEditViewModel FromCustomer(Customer c) {
            return new ProfileEditViewModel 
            {
                Name = c.Name,
                Address = c.Address,
                City = c.City,
                State = c.State,
                PostCode = c.PostCode,
                Phone = c.Phone,
                TFN = c.TFN
            };
        }

        public void Validate(ModelStateDictionary modelState)
        {
            Password = Password.Trim();
            if (string.IsNullOrEmpty(Password))
            {
                modelState.AddModelError("Password", 
                    "Password is required and cannot be white spaces.");
            }
        }

        public Customer GenerateCustomer() {
            return new Customer 
            {
                Name = Name,
                Address = Address,
                City = City,
                State = State,
                PostCode = PostCode,
                Phone = Phone,
                TFN = TFN
            };
        }
    }
}
