using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using BankingLib.Data;
using BankingLib.Models;
using BankingAdmin.Models.Repository;

namespace BankingAdmin.Models.Manager
{
    public class BillPayManager : IAsyncRepository<BillPay, int>
    {
        private readonly BankingContext _context;
        private readonly DbSet<BillPay> _set;

        public BillPayManager(BankingContext context)
        {
            _context = context;
            _set = _context.BillPay;
        }


        public async Task<IEnumerable<BillPay>> GetManyAsync(Expression<Func<BillPay, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<BillPay>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<BillPay> GetAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<int> AddAsync(BillPay item)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(int id, BillPay item)
        {
            item.BillPayID = id;
            if (_set.Local.FirstOrDefault(x => x.BillPayID == item.BillPayID) == null)
                _set.Update(item);
            await _context.SaveChangesAsync();
            return item.BillPayID;
        }

        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
