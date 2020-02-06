using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BankingLib.Models;
using BankingAdmin.Models.Repository;

namespace BankingAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IAsyncRepository<Customer, int> _repo;
        public CustomerController(IAsyncRepository<Customer, int> repo) 
        {
            _repo = repo;
        }
        private static Customer TrimCustomer(Customer c)
        {
            if (c != null)
                return new Customer
                {
                    CustomerID = c.CustomerID,
                    Name = c.Name,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    PostCode = c.PostCode,
                    Phone = c.Phone,
                    TFN = c.TFN
                };
            return c;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return (await _repo.GetAllAsync()).Select(x => TrimCustomer(x));
        }

        [HttpGet("{customerId}")]
        public async Task<Customer> Get(int customerId)
        {
            return TrimCustomer(await _repo.GetAsync(customerId));
        }

        [HttpPost("Add")]
        public async Task<int> Post([FromBody] [Bind("Name,Address,City,State,PostCode,Phone,Tfn")] Customer customer)
        {
            return await _repo.AddAsync(customer);
        }

        [HttpPut("Edit/{customerId}")]
        public async Task<int> Put(int customerId, [FromBody] Customer customer)
        {
            return await _repo.UpdateAsync(customerId, customer);
        }

        [HttpDelete("Delete/{customerId}")]
        public async Task<int> Delete(int customerId)
        {
            return await _repo.DeleteAsync(customerId);
        }
    }
}
