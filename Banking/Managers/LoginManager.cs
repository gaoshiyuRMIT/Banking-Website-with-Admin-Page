using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

using Banking.Models;
using Banking.Data;


namespace Banking.Managers
{
    public interface ILoginManager 
    {
        public Task<Login> GetLoginAsync(string userId);
        public Task UpdateAsync(Login login, string userId);
        public Task UpdatePasswordAsync(Login login, string password);
        public Task<Login> GetLoginForCustomerAsync(int customerId);
    }

    public class LoginManager : ILoginManager
    {
        private BankingContext _context;
        private DbSet<Login> _set;
        
        public LoginManager(BankingContext context) 
        {
            _context = context;
            _set = _context.Login;
        }

        public async Task<Login> GetLoginAsync(string userId) 
        {
            return await _set.FindAsync(userId);
        }
        public async Task UpdateAsync(Login login, string userId) 
        {
            login.UserID = userId;
            _context.Update(login);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePasswordAsync(Login login, string password)
        {
            login.PasswordHash = PBKDF2.Hash(password);
            await _context.SaveChangesAsync();
        }
        public async Task<Login> GetLoginForCustomerAsync(int customerId)
        {
            return await _set.FirstOrDefaultAsync(x => x.CustomerID == customerId);
        }

    }
}
