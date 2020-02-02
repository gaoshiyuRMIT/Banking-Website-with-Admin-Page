using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BankingAdmin.Models.Manager;
using BankingAdmin.Models.Repository;
using BankingAdmin.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        public readonly IAsyncSearchRepository<Transaction, TransactionQuery> _repo;

        public TransactionController(IAsyncSearchRepository<Transaction, TransactionQuery> repo) 
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Transaction>> Get([FromQuery] TransactionQuery query)
        {
            return await _repo.GetManyByQueryAsync(query);
        }

    }
}
