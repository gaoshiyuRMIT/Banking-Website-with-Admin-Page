using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using System.Collections.Generic;

using BankingLib.Models;

namespace Banking.ViewModels
{
    public class BasicOpViewModel : IViewModel
    {
        private List<SelectListItem> _accountTypes = new List<SelectListItem>();
        private Account _account;
        private AccountType _accountType;
        private Customer _customer;

        public Customer Customer {
            get => _customer;
            set
            {
                _customer = value;
                // account & account type list needs to update
                _accountTypes.Clear();
                _account = null;
            }

        }
        [Display(Name = "Account Types")]
        public List<SelectListItem> AccountTypes {
            get {
                if (_accountTypes.Count == 0 && Customer != null)
                {
                    var accountTypes = from account in Customer.Accounts
                                       select account.AccountType;
                    foreach (var type in accountTypes)
                        _accountTypes.Add(new SelectListItem
                        {
                            Text = type.ToString(),
                            Value = type.ToString()
                        });
                }
                return _accountTypes;
            }
            set => _accountTypes = value;
        }

        [Display(Name = "Account Type")]
        public AccountType AccountType {
            get => _accountType;
            set
            {
                _accountType = value;
                // account needs to update
                _account = null;
            }
        }
        public Account Account
        {
            set => _account = value;
            get
            {
                if (_account == null && Customer != null)
                {
                    return Customer.Accounts.Find(x => x.AccountType == AccountType);
                }
                return _account;
            }
        }

        public virtual void Validate(ModelStateDictionary modelState) {}
        

        public virtual void Clear() 
        {
        }

    }
}
