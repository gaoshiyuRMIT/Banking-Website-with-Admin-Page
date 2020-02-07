using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BankingAdmin.Models.Manager;
using BankingAdmin.Models.Repository;
using BankingLib.Models;


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


        private static Transaction TrimTransaction(Transaction t)
        {
            return new Transaction
            {
                TransactionID = t.TransactionID,
                TransactionType = t.TransactionType,
                AccountNumber = t.AccountNumber,
                DestAccountNumber = t.DestAccountNumber,
                Amount = t.Amount,
                Comment = t.Comment
            };
        }


        [HttpGet]
        public async Task<IEnumerable<Transaction>> Get([FromQuery] TransactionQuery query)
        {
            return (await _repo.GetManyByQueryAsync(query)).Select(x => TrimTransaction(x));
        }
        
    }
}
