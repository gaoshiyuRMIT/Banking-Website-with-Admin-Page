using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


using Banking.Attributes;
using Banking.Data;
using BankingLib.Models;
using Banking.Models;
using Banking.ViewModels;
using Banking.Managers;

namespace Banking.Controllers
{

    [AuthorizeCustomer]
    [Route("Customer/BillPay/Payee")]
    public class PayeeController : Controller
    {
        private IPayeeManager PMgr {get;}

        private int CustomerID => CustomerSessionKey.GetCustomerID(HttpContext.Session).Value;

        public PayeeController(IPayeeManager pmgr)
        {
            PMgr = pmgr;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var payees = PMgr.GetPayeesOfCustomer(CustomerID);
            return View(payees);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] int payeeID)
        {
            var payee = await PMgr.GetPayeeAsync(payeeID);

            var viewModel = BillPaySessionKey.GetEditViewModelFromSession(HttpContext.Session);
            viewModel.Payee = payee;
            BillPaySessionKey.SetEditViewModelToSession(viewModel, HttpContext.Session);

            if (viewModel.BillPayEditOp == BillPayEditOp.Create)
                return RedirectToAction("Create", "BillPay");
            return RedirectToAction("Edit", "BillPay");
        }

        [Route("Create")]
        public IActionResult Create() => View();

        [Route("Create")]
        [HttpPost]
        public IActionResult Create([Bind("Name,Address,City,State,PostCode,Phone")] Payee payee)
        {
            if (!ModelState.IsValid)
                return View(payee);

            var viewModel = BillPaySessionKey.GetEditViewModelFromSession(HttpContext.Session);
            viewModel.Payee = payee;
            BillPaySessionKey.SetEditViewModelToSession(viewModel, HttpContext.Session);

            if (viewModel.BillPayEditOp == BillPayEditOp.Create)
                return RedirectToAction("Create", "BillPay");
            return RedirectToAction("Edit", "BillPay");
        }

        [Route("Cancel")]
        public IActionResult Cancel()
		{
            var viewModel = BillPaySessionKey.GetEditViewModelFromSession(HttpContext.Session);
            if (viewModel.BillPayEditOp == BillPayEditOp.Create)
                return RedirectToAction("Create", "BillPay");
            return RedirectToAction("Edit", "BillPay");
        }
    }
}
