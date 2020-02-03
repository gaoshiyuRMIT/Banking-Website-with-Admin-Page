using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

using Banking.Models;
using Banking.Data;



namespace Banking.Managers
{
    
    public interface IPayeeManager 
    {
        public Task<Payee> GetPayeeAsync(int payeeID);
        public IEnumerable<Payee> GetPayeesOfCustomer(int customerID);
    }
    public class PayeeManager : IPayeeManager
    {    
        private BankingContext _context;
        private DbSet<Payee> _set;

        public PayeeManager(BankingContext context) 
        {
            _context = context;
            _set = _context.Payee;
        }

        public async Task<Payee> GetPayeeAsync(int payeeID) {
            return await _set.FindAsync(payeeID);
        }

        public IEnumerable<Payee> GetPayeesOfCustomer(int customerID)
        {
            var payees = from account in _context.Account
                            join billPay in _context.BillPay
                            on account.AccountNumber equals billPay.AccountNumber
                        where account.CustomerID == customerID
                        select billPay.Payee;
            payees = payees.Distinct<Payee>().OrderBy<Payee, string>(x => x.Name);
            return payees;
        }

    }
}
