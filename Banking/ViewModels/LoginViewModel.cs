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

        public void Validate(ModelStateDictionary modelState)
        {
            if (Login == null || !PBKDF2.Verify(Login.PasswordHash, Password))
            {
                modelState.AddModelError("LoginFailed", "Login failed, please try again.");
            }
        }
        public void Clear() {}
    }

}
