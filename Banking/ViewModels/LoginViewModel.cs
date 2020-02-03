using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleHashing;


using Banking.Models;

namespace Banking.ViewModels
{
    public class LoginViewModel : IViewModel
    {
        public Login Login { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }

        private bool _authFailed = false;
        public bool AuthFailed => _authFailed;

        public void Validate(ModelStateDictionary modelState)
        {
            if (Login?.IsLocked == true)
            {
                modelState.AddModelError("Login.UserID", "Account locked, please try later.");
                return;
            }
            _authFailed = !Login.Verify(Password);
            if (Login == null || AuthFailed)
                modelState.AddModelError("LoginFailed", "Login failed, please try again.");
        }
        public void Clear() {}
    }

}
