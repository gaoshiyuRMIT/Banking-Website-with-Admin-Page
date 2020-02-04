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
    public interface ICustomerManager
    {
        public Task<Customer> GetCustomerAsync(int CustomerID);
        public Task UpdateAsync(Customer customer, int customerID);

    }

    public class CustomerManager : ICustomerManager
    {
        private BankingContext _context;
        private DbSet<Customer> _set;

        public CustomerManager(BankingContext context)
        {
            _context = context;
            _set = _context.Customer;
        }

        public async Task<Customer> GetCustomerAsync(int CustomerID)
        {
            return await _set.FindAsync(CustomerID);
        }

        public async Task UpdateAsync(Customer customer, int customerID)
        {
            customer.CustomerID = customerID;
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
