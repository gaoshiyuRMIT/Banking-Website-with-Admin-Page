using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

using BankingLib.Models;
using BankingLib.Data;


namespace Banking.Managers
{
    public interface ILoginManager 
    {
        public Task<Login> GetLoginAsync(string userId);
        public Task UpdateAsync(Login login, string userId);
        public Task UpdatePasswordAsync(Login login, string password);
        public Task<Login> GetLoginForCustomerAsync(int customerId);
        public Task IncrementAttemptsAsync(Login login);
        public Task UpdateLockAsync(Login login);

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

        public async Task IncrementAttemptsAsync(Login login)
        {
            login.Attempts += 1;
            await _context.SaveChangesAsync();
            if (login.Attempts >= 3)
            {
                login.Lock();
                login.Attempts = 0;
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task UpdateLockAsync(Login login)
        {   
            if (login.LockExpired)
            {
                login.Unlock();
                await _context.SaveChangesAsync();
            }
        }
    }
}
