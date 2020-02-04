using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BankingLib.Models;
using Banking.Models;
using Banking.ViewModels;
using Banking.Data;
using Banking.Attributes;
using Banking.Managers;

using X.PagedList;


namespace Banking.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private const string statementsResultSessionKey = "StatementsResultAccountNumber";
        private IAccountManager AMgr { get; }
        private ICustomerManager CMgr { get; }
        private ILoginManager LMgr { get; }

        private int CustomerID => CustomerSessionKey.GetCustomerID(HttpContext.Session).Value;

        public CustomerController(IAccountManager accountManager, ICustomerManager customerManager,
            ILoginManager loginManager)
        {
            AMgr = accountManager;
            CMgr = customerManager;
            LMgr = loginManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            return View(customer);
        }

        public async Task<IActionResult> Withdraw()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            var viewModel = new WithdrawViewModel
            {
                Customer = customer
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawViewModel viewModel)
        {
            viewModel.Customer = await CMgr.GetCustomerAsync(CustomerID);
            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View(viewModel);

            await AMgr.WithdrawAsync(viewModel.Account, viewModel.Amount, viewModel.Comment);

            viewModel.OperationStatus = OperationStatus.Successful;
            viewModel.Clear();
            return View(viewModel);
        }

        public async Task<IActionResult> Deposit()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            var viewModel = new DepositViewModel
            {
                Customer = customer
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel viewModel)
        {
            viewModel.Customer = await CMgr.GetCustomerAsync(CustomerID);
            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View(viewModel);

            await AMgr.DepositAsync(viewModel.Account, viewModel.Amount, viewModel.Comment);

            viewModel.OperationStatus = OperationStatus.Successful;
            viewModel.Clear();
            return View(viewModel);
        }

        public async Task<IActionResult> Transfer()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            var viewModel = new TransferViewModel
            {
                Customer = customer
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel viewModel)
        {
            viewModel.Customer = await CMgr.GetCustomerAsync(CustomerID);
            viewModel.DestAccount = await AMgr.GetAccountAsync(viewModel.DestAccountNumber);
            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View(viewModel);

            await AMgr.TransferAsync(viewModel.Account, viewModel.DestAccount, viewModel.Amount, viewModel.Comment);

            viewModel.OperationStatus = OperationStatus.Successful;
            viewModel.Clear();
            return View(viewModel);
        }

        public async Task<IActionResult> Statements()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            var viewModel = new BasicOpViewModel
            {
                Customer = customer
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Statements(BasicOpViewModel viewModel)
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            viewModel.Customer = customer;

            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View(viewModel);

            HttpContext.Session.SetInt32(statementsResultSessionKey, viewModel.Account.AccountNumber);

            return RedirectToAction("StatementsResult");
        }

        public async Task<IActionResult> StatementsResult(int? page = 1)
        {
            int? accountNumber = HttpContext.Session.GetInt32(statementsResultSessionKey);
            if (accountNumber == null)
                return RedirectToAction("Statements");

            var account = await AMgr.GetAccountAsync(accountNumber.Value);
            IPagedList<Transaction> transactions = await AMgr.GetPagedTransactionsAsync(accountNumber.Value, page);
            return View(new StatementsResultViewModel {
                Transactions = transactions, Account = account });
        }

        public async Task<IActionResult> Profile()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            return View(customer);
        }

        [Route("Profile/Edit")]
        public async Task<IActionResult> ProfileEdit()
        {
            var customer = await CMgr.GetCustomerAsync(CustomerID);
            var viewModel = ProfileEditViewModel.FromCustomer(customer);
            return View(viewModel);
        }

        [HttpPost]
        [Route("Profile/Edit")]
        public async Task<IActionResult> ProfileEdit(
            [Bind("Name,TFN,Address,City,State,PostCode,Phone")] ProfileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var customer = viewModel.GenerateCustomer();
            await CMgr.UpdateAsync(customer, CustomerID);
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Route("Profile/Edit/Password")]
        public async Task<IActionResult> EditPassword(ProfileEditViewModel viewModel)
        {
            viewModel.Validate(ModelState);
            if (!ModelState.IsValid)
                return View("ProfileEdit", viewModel);
            var login = await LMgr.GetLoginForCustomerAsync(CustomerID);
            await LMgr.UpdatePasswordAsync(login, viewModel.Password);

            viewModel.Password = null;
            viewModel.OperationStatus = OperationStatus.Successful;
            return View("ProfileEdit", viewModel);
        }
    }
}
