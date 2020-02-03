using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View(viewModel);

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
