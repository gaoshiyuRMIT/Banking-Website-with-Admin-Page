using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using BankingLib.Data;
using BankingLib.Models;
using BankingLib.Extensions;
using BankingAdmin.Models.Repository;

namespace BankingAdmin.Models.Manager
{
    public class TransactionQuery
    {
        public int? TransactionId { get; set; }
        public string ModifyDateFrom { get; set; }
        public string ModifyDateTo { get; set; }
        public TransactionType? TransactionType { get; set; }
        public int? AccountNumber { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public string CommentContains { get;set; }

        public DateTime? ModifyDateFromDT => ModifyDateFrom?.ToDateTime();
        public DateTime? ModifyDateToDT => ModifyDateTo?.ToDateTime();

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
            List<Expression<Func<Transaction, bool>>> predicates = new List<Expression<Func<Transaction, bool>>>();
            if (query.TransactionId != null)
                predicates.Add(x => x.TransactionID == query.TransactionId.Value);
            if (query.ModifyDateFromDT != null)
                predicates.Add(x => x.ModifyDate >= query.ModifyDateFromDT.Value);
            if (query.ModifyDateToDT != null)
                predicates.Add(x => x.ModifyDate <= query.ModifyDateToDT.Value);
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
            return await GetManyAsync(predicates);
        }
        
        public async Task<IEnumerable<Transaction>> GetManyAsync(
            IEnumerable<Expression<Func<Transaction, bool>>> predicateList)
        {
            IQueryable<Transaction> transactions = _set;
            foreach (var p in predicateList) 
            {
                transactions = transactions.Where(p);
            }
            return await transactions.ToListAsync();
        }

    }
}
