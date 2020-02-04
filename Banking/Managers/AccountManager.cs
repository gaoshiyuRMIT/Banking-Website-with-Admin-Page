using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using BankingLib.Models;
using BankingLib.Data;

namespace Banking.Managers
{
    public interface IAccountManager
    {
        public Task WithdrawAsync(Account account, decimal amount, string comment);
        public Task DepositAsync(Account account, decimal amount, string comment);
        public Task TransferAsync(Account account, Account destAccount, decimal amount, string comment);
        public Task<IPagedList<Transaction>> GetPagedTransactionsAsync(int accountNumber, int? page = 1);
        public Task<Account> GetAccountAsync(int accountNumber);
    }

    public class AccountManager : IAccountManager
    {
        private BankingContext _context;
        private DbSet<Account> _set;
        private const int statementsPageSize = 4;

        public AccountManager(BankingContext context)
        {
            _context = context;
            _set = context.Account;
        }

        public async Task WithdrawAsync(Account account, decimal amount, string comment)
        {
            account.Withdraw(amount, comment);
            await _context.SaveChangesAsync();
        }

        public async Task DepositAsync(Account account, decimal amount, string comment)
        {
            account.Deposit(amount, comment);
            await _context.SaveChangesAsync();
        }

        public async Task TransferAsync(Account account, Account destAccount, decimal amount, string comment)
        {
            account.Transfer(destAccount, amount, comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<Transaction>> GetPagedTransactionsAsync(int accountNumber, int? page = 1)
        {
            var transactions = _context.Transaction
                .Where(x => x.AccountNumber == accountNumber)
                .OrderByDescending<Transaction, DateTime>(x => x.ModifyDate);
            var pagedList = await transactions
                .ToPagedListAsync<Transaction>(page, statementsPageSize);
            return pagedList;
        }

        public async Task<Account> GetAccountAsync(int accountNumber)
        {
            return await _set.FindAsync(accountNumber);
        }
    }
}
