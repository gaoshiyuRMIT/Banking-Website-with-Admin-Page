using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BankingAdmin.Models.Repository
{
    public interface IAsyncSearchRepository<TEntity, TQuery>
    {
        Task<IEnumerable<TEntity>> GetManyByQueryAsync(TQuery query);
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<Transaction, bool>>[] predicateList);

    }
}
