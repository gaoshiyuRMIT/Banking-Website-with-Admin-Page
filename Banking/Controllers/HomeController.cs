using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankingLib.Models;
using Banking.Models;
using Banking.ViewModels;
using Microsoft.AspNetCore.Diagnostics;

namespace Banking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int? customerIdNullable = CustomerSessionKey.GetCustomerID(HttpContext.Session);
            if (customerIdNullable == null)
                return RedirectToAction("Index", "Login");
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Error(int? errorCode)
        {
            var viewModel = new ErrorViewModel();
            if (errorCode == null)
            {
                var exceptionHandlerPathFeature =
                    HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature?.Error is Exception)
                    viewModel.Code = 500;
            }
            else 
                viewModel.Code = errorCode.Value;
            return View(viewModel);
        }

    }
}
