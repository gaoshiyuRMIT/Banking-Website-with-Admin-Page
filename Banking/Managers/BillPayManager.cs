using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

using BankingLib.Models;
using BankingLib.Data;

namespace Banking.Managers
{
    public interface IBillPayManager
    {
        public IEnumerable<BillPay> GetBillPaysOfCustomer(int customerID);
        public Task<BillPay> GetBillPayAsync(int billPayId);
        public Task UpdateAsync(BillPay billPay, int billPayId);
        public Task AddToAccountAsync(Account account, BillPay billPay);
    }

    public class BillPayManager : IBillPayManager
    {        
        private BankingContext _context;
        private DbSet<BillPay> _set;

        public BillPayManager(BankingContext context)
        {
            _context = context;
            _set = _context.BillPay;
        }

        public IEnumerable<BillPay> GetBillPaysOfCustomer(int customerID) 
        {
            IEnumerable<BillPay> billPays = _context.BillPay
                .Where(x => x.Account.CustomerID == customerID)
                .OrderBy<BillPay, DateTime>(x => x.ScheduleDate);
            return billPays;
        }

        public async Task<BillPay> GetBillPayAsync(int billPayId) 
        {
            return await _set.FindAsync(billPayId);
        }

        public async Task UpdateAsync(BillPay billPay, int billPayId) 
        {
            billPay.BillPayID = billPayId;
            billPay.ModifyDate = DateTime.UtcNow;
            _set.Update(billPay);
            await _context.SaveChangesAsync();
        }

        public async Task AddToAccountAsync(Account account, BillPay billPay) 
        {
            billPay.ModifyDate = DateTime.UtcNow;
            account.BillPays.Add(billPay);
            await _context.SaveChangesAsync();
        }
    }
}
