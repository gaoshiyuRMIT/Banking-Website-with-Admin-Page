using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using BankingLib.Models;
using Banking.Models;
using Banking.ViewModels;
using Banking.Managers;

namespace Banking.Controllers
{
    public class LoginController : Controller
    {
        private ILoginManager LMgr { get; }

        public IActionResult Index() =>
            View(new LoginViewModel
            {
                Login = new Login
                {
                    PasswordHash = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                }
            });

        public LoginController(ILoginManager lmgr)
        {
            LMgr = lmgr;
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel viewModel)
        {
            viewModel.Login = await LMgr.GetLoginAsync(viewModel.Login.UserID);
            if (viewModel.Login?.IsLocked == true)
                await LMgr.UpdateLockAsync(viewModel.Login);

            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
            {
                if (viewModel.AuthFailed)
                    await LMgr.IncrementAttemptsAsync(viewModel.Login);
                return View(viewModel);
            }
            
            await LMgr.ClearAttemptsAsync(viewModel.Login);
            CustomerSessionKey.SetToSession(viewModel.Login.Customer, HttpContext.Session);

            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}
