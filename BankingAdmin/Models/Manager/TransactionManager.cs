using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

using BankingAdmin.Data;
using BankingAdmin.Models;
using BankingAdmin.Models.Repository;

namespace BankingAdmin.Models.Manager
{
    public class TransactionQuery
    {
        public int? TransactionId { get; set; }
        public DateTime? ModifyDateFrom { get; set; }
        public DateTime? ModifyDateTo { get; set; }
        public int? TransactionType { get; set; }
        public int? AccountNumber { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public string CommentContains { get;set; }
    }

    public class TransactionManager : IAsyncSearchRepository<Transaction, TransactionQuery>
    {
        private readonly BankingContext _context;
        private readonly DbSet<Transaction> _set;

        public TransactionManager(BankingContext context) 
        {
            _context = context;
            _set = _context.Transaction;
        }

        public async Task<IEnumerable<Transaction>> GetManyByQueryAsync(TransactionQuery query)
        {
            List<Func<Transaction, bool>> predicates = new List<Func<Transaction, bool>>();
            if (query.TransactionId != null)
                predicates.Add(x => x.TransactionId == query.TransactionId.Value);
            if (query.ModifyDateFrom != null)
                predicates.Add(x => x.ModifyDate >= query.ModifyDateFrom.Value);
            if (query.ModifyDateTo != null)
                predicates.Add(x => x.ModifyDate <= query.ModifyDateTo.Value);
            if (query.TransactionType != null)
                predicates.Add(x => x.TransactionType == query.TransactionType.Value);
            if (query.AccountNumber != null)
                predicates.Add(x => x.AccountNumber == query.AccountNumber.Value);
            if (query.AmountFrom != null)
                predicates.Add(x => x.Amount >= query.AmountFrom.Value);
            if (query.AmountTo != null)
                predicates.Add(x => x.Amount <= query.AmountTo.Value);
            if (query.CommentContains != null)
                predicates.Add(x => x.Comment.Contains(query.CommentContains));
            return await GetManyAsync(predicates.ToArray());
        }
        
        public async Task<IEnumerable<Transaction>> GetManyAsync(
            Func<Transaction, bool>[] predicateList)
        {
            Func<Transaction, bool> f = t => {
                foreach (var predicate in predicateList)
                {
                    if (!predicate(t))
                        return false;
                }
                return true;
            };
            var transactions = from transaction in _set
                                where f(transaction)
                                select transaction;
            return await transactions.ToListAsync();
        }

    }
}
