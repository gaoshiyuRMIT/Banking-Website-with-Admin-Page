using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using BankingAdmin.Data;
using BankingAdmin.Models;
using BankingAdmin.Models.Repository;


namespace BankingAdmin.Models.Manager {
    delegate Customer CustomerTransform(Customer c);

    public class CustomerManager : IAsyncRepository<Customer, int>
    {
        private readonly BankingContext _context;
        private readonly DbSet<Customer> _set;

        public CustomerManager(BankingContext context)
        {
            _context = context;
            _set = _context.Customer;
        }

        private static Customer TrimCustomer(Customer c)
        {
            if (c != null)
                return new Customer
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    PostCode = c.PostCode,
                    Phone = c.Phone,
                    Tfn = c.Tfn
                };
            return c;
        }

        public async Task<Customer> GetAsync(int customerId)
        {
            return TrimCustomer(await _set.FindAsync(customerId));
        }

        public async Task<IEnumerable<Customer>> GetManyAsync(Func<Customer, bool> predicate)
        {
            var customers = from customer in _set
                            where predicate(customer)
                            select TrimCustomer(customer);
            return await customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = from customer in _set select TrimCustomer(customer);
            return await customers.ToListAsync();
        }

        public async Task<int> UpdateAsync(int customerId, Customer customer)
        {
            customer.CustomerId = customerId;
            _set.Update(customer);
            await _context.SaveChangesAsync();
            return customerId;
        }

        public async Task<int> DeleteAsync(int customerId)
        {
            var customer = await _set.FindAsync(customerId);
            _set.Remove(customer);
            await _context.SaveChangesAsync();
            return customerId;
        }

        public async Task<int> AddAsync(Customer customer)
        {
            await _set.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer.CustomerId;
        }
    }
}
