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
    public class LoginManager : IAsyncRepository<Login, string>
    {
        private readonly BankingContext _context;
        private readonly DbSet<Login> _set;

        public LoginManager(BankingContext context)
        {
            _context = context;
            _set = _context.Login;
        }

        public async Task<IEnumerable<Login>> GetManyAsync(Expression<Func<Login, bool>> predicate)
        {
            var logins = _set.Where(predicate);
            return await logins.ToListAsync();
        }

        public async Task<IEnumerable<Login>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Login> GetAsync(string userId)
        {
            return await _set.FindAsync(userId);
        }

        public async Task<string> AddAsync(Login login)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(string id, Login login)
        {
            login.UserID = id;
            if (_set.Local.FirstOrDefault(x => x.UserID == id) == null)
                _set.Update(login);
            await _context.SaveChangesAsync();
            return login.UserID;
        }
        public async Task<string> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

    }
}
